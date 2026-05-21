using System.Collections.ObjectModel;
using System.Windows.Input;
using RepoNotes.App.Services;
using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.App.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private INoteRepository _noteRepository;
    private readonly IFolderPickerService _folderPickerService;
    private readonly MarkdownPreviewService _markdownPreviewService;
    private readonly InternalLinkService _internalLinkService;
    private readonly IRepositorySettingsStore _settingsStore;
    private readonly INoteTemplateService _noteTemplateService;
    private readonly ITextPromptService _textPromptService;
    private readonly Func<string?, INoteRepository> _noteRepositoryFactory;
    private readonly TimeSpan _searchDebounceDelay;
    private NoteItem? _selectedNote;
    private RepositoryNodeViewModel? _selectedNode;
    private TrashItem? _selectedTrashItem;
    private string _repositoryName;
    private string _repositoryPath;
    private string _searchText = string.Empty;
    private string _appliedSearchText = string.Empty;
    private string _searchFeedback = "Digite para buscar em titulo, caminho e conteudo";
    private string _selectedTag = string.Empty;
    private string _status = "Salvo";
    private string _lastErrorMessage = string.Empty;
    private bool _hasUnsavedChanges;
    private int _searchResultCount;
    private CancellationTokenSource? _searchDebounceCancellation;
    private NoteTemplate _selectedTemplate;
    private readonly IReadOnlyList<string> _statusOptions = ["Draft", "Active", "Review", "Archived"];

    public MainWindowViewModel(
        INoteRepository noteRepository,
        IFolderPickerService? folderPickerService = null,
        IRepositorySettingsStore? settingsStore = null,
        Func<string?, INoteRepository>? noteRepositoryFactory = null,
        string? initialStatus = null,
        MarkdownPreviewService? markdownPreviewService = null,
        InternalLinkService? internalLinkService = null,
        INoteTemplateService? noteTemplateService = null,
        ITextPromptService? textPromptService = null,
        TimeSpan? searchDebounceDelay = null)
    {
        _noteRepository = noteRepository;
        _folderPickerService = folderPickerService ?? new NullFolderPickerService();
        _markdownPreviewService = markdownPreviewService ?? new MarkdownPreviewService();
        _internalLinkService = internalLinkService ?? new InternalLinkService();
        _settingsStore = settingsStore ?? new NullRepositorySettingsStore();
        _noteTemplateService = noteTemplateService ?? new TechnicalNoteTemplateService();
        _textPromptService = textPromptService ?? new NullTextPromptService();
        _searchDebounceDelay = searchDebounceDelay ?? TimeSpan.FromMilliseconds(250);
        Templates = _noteTemplateService.GetTemplates();
        _selectedTemplate = _noteTemplateService.GetDefaultTemplate();
        _noteRepositoryFactory = noteRepositoryFactory ?? (_ => noteRepository);
        _repositoryName = noteRepository.CurrentRepository.Name;
        _repositoryPath = noteRepository.CurrentRepository.RootPath;
        Nodes = [];
        PreviewBlocks = [];
        InternalLinks = [];
        TrashItems = [];
        TagFilters = [];

        NewNoteCommand = new AsyncRelayCommand(CreateNewNoteAsync);
        NewFromTemplateCommand = new AsyncRelayCommand(CreateNewNoteFromSelectedTemplateAsync);
        NewFolderCommand = new AsyncRelayCommand(CreateNewFolderAsync);
        OpenFavoritesCommand = new RelayCommand(() => Status = "Favoritos ainda nao implementados no MVP");
        OpenRepositoryCommand = new AsyncRelayCommand(OpenRepositoryAsync);
        OpenSettingsCommand = new RelayCommand(() => Status = "Configuracoes ainda nao implementadas no MVP");
        ClearSearchCommand = new RelayCommand(ClearSearch);
        ClearTagFilterCommand = new RelayCommand(ClearTagFilter);
        RenameSelectedItemCommand = new AsyncRelayCommand(RenameSelectedItemAsync);
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        RestoreFromTrashCommand = new RelayCommand(RestoreFromTrash);
        DeletePermanentlyCommand = new RelayCommand(DeletePermanently);
        EmptyTrashCommand = new RelayCommand(EmptyTrash);
        SaveNoteCommand = new RelayCommand(SaveSelectedNote, () => SelectedNote is not null);

        ReloadRepository(noteRepository, initialStatus);
    }

    public string AppName => "RepoNotes";

    public string RepositoryName
    {
        get => _repositoryName;
        private set => SetProperty(ref _repositoryName, value);
    }

    public string RepositoryPath
    {
        get => _repositoryPath;
        private set => SetProperty(ref _repositoryPath, value);
    }

    public ObservableCollection<RepositoryNodeViewModel> Nodes { get; }

    public ObservableCollection<MarkdownPreviewBlock> PreviewBlocks { get; }

    public ObservableCollection<InternalLinkViewModel> InternalLinks { get; }

    public ObservableCollection<TrashItem> TrashItems { get; }

    public ObservableCollection<TagFilterViewModel> TagFilters { get; }

    public IReadOnlyList<NoteTemplate> Templates { get; }

    public IReadOnlyList<string> StatusOptions => _statusOptions;

    public NoteTemplate SelectedTemplate
    {
        get => _selectedTemplate;
        set
        {
            if (value is not null)
            {
                SetProperty(ref _selectedTemplate, value);
                OnPropertyChanged(nameof(SelectedTemplateDescription));
            }
        }
    }

    public string SelectedTemplateDescription => SelectedTemplate.Description;

    public ICommand NewNoteCommand { get; }

    public ICommand NewFromTemplateCommand { get; }

    public ICommand NewFolderCommand { get; }

    public ICommand OpenFavoritesCommand { get; }

    public ICommand OpenRepositoryCommand { get; }

    public ICommand OpenSettingsCommand { get; }

    public ICommand ClearSearchCommand { get; }

    public ICommand ClearTagFilterCommand { get; }

    public ICommand RenameSelectedItemCommand { get; }

    public ICommand DeleteSelectedItemCommand { get; }

    public ICommand RestoreFromTrashCommand { get; }

    public ICommand DeletePermanentlyCommand { get; }

    public ICommand EmptyTrashCommand { get; }

    public ICommand SaveNoteCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (!SetProperty(ref _searchText, value))
            {
                return;
            }

            OnPropertyChanged(nameof(HasSearchText));
            ScheduleSearchRefresh();
        }
    }

    public bool HasSearchText => !string.IsNullOrWhiteSpace(SearchText);

    public string SearchFeedback
    {
        get => _searchFeedback;
        private set => SetProperty(ref _searchFeedback, value);
    }

    public bool HasNoSearchResults => IsSearchFiltering && _searchResultCount == 0;

    private bool IsSearchFiltering => !string.IsNullOrWhiteSpace(_appliedSearchText) || !string.IsNullOrWhiteSpace(SelectedTag);

    public bool HasInternalLinks => InternalLinks.Count > 0;

    public string SelectedTag
    {
        get => _selectedTag;
        private set
        {
            if (!SetProperty(ref _selectedTag, value))
            {
                return;
            }

            OnPropertyChanged(nameof(HasTagFilter));
            RefreshTagFilters();
        }
    }

    public bool HasTagFilter => !string.IsNullOrWhiteSpace(SelectedTag);

    public bool HasTags => TagFilters.Count > 0;

    public RepositoryNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        set
        {
            if (ReferenceEquals(_selectedNode, value))
            {
                return;
            }

            if (value?.NoteId is not null && !TrySaveSelectedNote())
            {
                OnPropertyChanged();
                return;
            }

            if (!SetProperty(ref _selectedNode, value) || value?.NoteId is null)
            {
                return;
            }

            SelectedNote = _noteRepository.GetNoteById(value.NoteId);
        }
    }

    public TrashItem? SelectedTrashItem
    {
        get => _selectedTrashItem;
        set => SetProperty(ref _selectedTrashItem, value);
    }

    public NoteItem? SelectedNote
    {
        get => _selectedNote;
        private set
        {
            if (!SetProperty(ref _selectedNote, value))
            {
                return;
            }

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Markdown));
            OnPropertyChanged(nameof(PreviewText));
            OnPropertyChanged(nameof(NotePath));
            OnPropertyChanged(nameof(WordCountText));
            OnPropertyChanged(nameof(UpdatedAtText));
            OnPropertyChanged(nameof(CreatedAtText));
            OnPropertyChanged(nameof(TagsText));
            OnPropertyChanged(nameof(NoteType));
            OnPropertyChanged(nameof(NoteStatus));
            OnPropertyChanged(nameof(MetadataTagsText));
            UpdatePreviewBlocks();
            RefreshTagFilters();
            _hasUnsavedChanges = false;
            LastErrorMessage = string.Empty;
            Status = "Salvo";
        }
    }

    public string Title
    {
        get => SelectedNote?.Title ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Title == value)
            {
                return;
            }

            SelectedNote.Title = value;
            MarkNoteChanged();
            OnPropertyChanged();
            OnPropertyChanged(nameof(PreviewText));
            UpdatePreviewBlocks();
        }
    }

    public string Markdown
    {
        get => SelectedNote?.Markdown ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Markdown == value)
            {
                return;
            }

            SelectedNote.Markdown = value;
            MarkNoteChanged();
            OnPropertyChanged();
            OnPropertyChanged(nameof(PreviewText));
            OnPropertyChanged(nameof(WordCountText));
            UpdatePreviewBlocks();
        }
    }

    public string PreviewText => SelectedNote is null
        ? "Selecione uma nota para visualizar."
        : Markdown;

    public string NotePath => SelectedNote?.Path ?? RepositoryPath;

    public string WordCountText => $"{SelectedNote?.WordCount ?? 0} palavras";

    public string UpdatedAtText => SelectedNote is null
        ? "-"
        : SelectedNote.UpdatedAt.ToString("dd/MM/yyyy HH:mm");

    public string CreatedAtText => SelectedNote is null
        ? "-"
        : SelectedNote.CreatedAt.ToString("dd/MM/yyyy HH:mm");

    public string TagsText => SelectedNote is null || SelectedNote.Tags.Count == 0
        ? "Sem tags"
        : string.Join(", ", SelectedNote.Tags);

    public string NoteType
    {
        get => SelectedNote?.Type ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Type == value)
            {
                return;
            }

            SelectedNote.Type = string.IsNullOrWhiteSpace(value) ? "note" : value.Trim();
            MarkNoteChanged();
            OnPropertyChanged();
        }
    }

    public string NoteStatus
    {
        get => SelectedNote?.Status ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Status == value)
            {
                return;
            }

            SelectedNote.Status = string.IsNullOrWhiteSpace(value) ? "draft" : value.Trim();
            MarkNoteChanged();
            OnPropertyChanged();
        }
    }

    public string MetadataTagsText
    {
        get => SelectedNote is null ? string.Empty : string.Join(", ", SelectedNote.Tags);
        set
        {
            if (SelectedNote is null)
            {
                return;
            }

            var tags = ParseTagsText(value);
            if (SelectedNote.Tags.SequenceEqual(tags, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            SelectedNote.Tags = tags;
            MarkNoteChanged();
            OnPropertyChanged();
            OnPropertyChanged(nameof(TagsText));
            RefreshTagFilters();
        }
    }

    public string Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public string LastErrorMessage
    {
        get => _lastErrorMessage;
        private set => SetProperty(ref _lastErrorMessage, value);
    }

    private void MarkNoteChanged()
    {
        if (SelectedNote is not null)
        {
            SelectedNote.UpdatedAt = DateTime.Now;
        }

        Status = "Alterado";
        LastErrorMessage = string.Empty;
        _hasUnsavedChanges = true;
        OnPropertyChanged(nameof(UpdatedAtText));
    }

    private void SaveSelectedNote()
    {
        _ = TrySaveSelectedNote();
    }

    private void UpdatePreviewBlocks()
    {
        PreviewBlocks.Clear();

        foreach (var block in _markdownPreviewService.Render(PreviewText))
        {
            PreviewBlocks.Add(block);
        }

        RefreshInternalLinks();
    }

    private void RefreshInternalLinks()
    {
        InternalLinks.Clear();

        if (SelectedNote is not null)
        {
            foreach (var link in _internalLinkService.ResolveLinks(SelectedNote.Markdown, _noteRepository.GetNotes()))
            {
                InternalLinks.Add(new InternalLinkViewModel(link, OpenInternalLink));
            }
        }

        OnPropertyChanged(nameof(HasInternalLinks));
    }

    private void OpenInternalLink(InternalLinkViewModel link)
    {
        if (!link.IsResolved || string.IsNullOrWhiteSpace(link.NoteId))
        {
            Status = $"Link interno nao encontrado: {link.Target}";
            return;
        }

        if (!TrySaveSelectedNote())
        {
            return;
        }

        OpenNoteById(link.NoteId);
        Status = $"Link interno aberto: {link.DisplayText}";
    }

    private async Task CreateNewNoteAsync()
    {
        await CreateNewNoteFromTemplateAsync(_noteTemplateService.GetDefaultTemplate(), "Nova nota", "Criar nova nota", "Nome da nota", "Nota criada");
    }

    private async Task CreateNewNoteFromSelectedTemplateAsync()
    {
        var noteName = GetTemplateNoteName(SelectedTemplate);
        await CreateNewNoteFromTemplateAsync(SelectedTemplate, noteName, "Criar nota por template", "Nome da nota", "Nota criada por template");
    }

    private async Task CreateNewNoteFromTemplateAsync(
        NoteTemplate template,
        string defaultNoteName,
        string promptTitle,
        string promptMessage,
        string successPrefix)
    {
        if (!TrySaveSelectedNote())
        {
            return;
        }

        var noteName = await PromptForNameAsync(promptTitle, promptMessage, defaultNoteName);
        if (noteName is null)
        {
            Status = "Criacao cancelada";
            return;
        }

        try
        {
            var note = _noteRepository.CreateNote(GetTargetFolderPath(), noteName, template);
            RefreshTree();
            RefreshTagFilters();
            SelectNodeByNoteId(note.Id);
            Status = $"{successPrefix}: {note.Path}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao criar nota";
        }
    }

    private async Task CreateNewFolderAsync()
    {
        if (!TrySaveSelectedNote())
        {
            return;
        }

        var folderName = await PromptForNameAsync("Criar nova pasta", "Nome da pasta", "Nova pasta");
        if (folderName is null)
        {
            Status = "Criacao cancelada";
            return;
        }

        try
        {
            var folderPath = _noteRepository.CreateFolder(GetTargetFolderPath(), folderName);
            RefreshTree();
            SelectNodeByPath(folderPath);
            Status = $"Pasta criada: {folderPath}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao criar pasta";
        }
    }

    private async Task RenameSelectedItemAsync()
    {
        if (SelectedNode is null)
        {
            Status = "Selecione uma nota ou pasta para renomear";
            return;
        }

        if (!TrySaveSelectedNote())
        {
            return;
        }

        var defaultName = GetPromptNameForSelectedNode(SelectedNode);
        var newName = await PromptForNameAsync("Renomear item", "Novo nome", defaultName);
        if (newName is null)
        {
            Status = "Renomeacao cancelada";
            return;
        }

        try
        {
            var wasNote = SelectedNode.IsNote;
            var newPath = _noteRepository.RenameItem(SelectedNode.Path, newName);
            RefreshTree();
            RefreshTagFilters();

            if (wasNote)
            {
                SelectNodeByNoteId(newPath);
            }
            else
            {
                SelectNodeByPath(newPath);
            }

            Status = $"Item renomeado: {newPath}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao renomear item";
        }
    }

    private void DeleteSelectedItem()
    {
        if (SelectedNode is null)
        {
            Status = "Raiz do repositorio nao pode ser excluida";
            return;
        }

        if (!TrySaveSelectedNote())
        {
            return;
        }

        var deletedPath = SelectedNode.Path;

        try
        {
            _noteRepository.MoveItemToTrash(deletedPath);
            RefreshTree();
            RefreshTagFilters();
            RefreshTrashItems();
            _selectedNode = null;
            OnPropertyChanged(nameof(SelectedNode));

            if (SelectedNote is null || IsPathInside(SelectedNote.Path, deletedPath))
            {
                SelectedNote = _noteRepository.GetNotes().FirstOrDefault();
            }

            Status = $"Item movido para a lixeira: {deletedPath}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao excluir item";
        }
    }

    private void RestoreFromTrash()
    {
        if (SelectedTrashItem is null)
        {
            Status = "Selecione um item da lixeira";
            return;
        }

        if (!TrySaveSelectedNote())
        {
            return;
        }

        try
        {
            var restoredPath = _noteRepository.RestoreFromTrash(SelectedTrashItem.TrashPath);
            RefreshTree();
            RefreshTagFilters();
            RefreshTrashItems();
            SelectNodeByPath(restoredPath);
            SelectNodeByNoteId(restoredPath);
            Status = $"Item restaurado: {restoredPath}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao restaurar item";
        }
    }

    private void DeletePermanently()
    {
        if (SelectedTrashItem is null)
        {
            Status = "Selecione um item da lixeira";
            return;
        }

        try
        {
            var deletedPath = SelectedTrashItem.TrashPath;
            _noteRepository.DeletePermanently(deletedPath);
            RefreshTrashItems();
            Status = $"Item excluido permanentemente: {deletedPath}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao excluir permanentemente";
        }
    }

    private void EmptyTrash()
    {
        try
        {
            _noteRepository.EmptyTrash();
            RefreshTrashItems();
            Status = "Lixeira esvaziada";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao esvaziar lixeira";
        }
    }

    private async Task OpenRepositoryAsync()
    {
        var selectedPath = await _folderPickerService.PickRepositoryPathAsync();
        if (string.IsNullOrWhiteSpace(selectedPath))
        {
            return;
        }

        if (!TryOpenRepository(selectedPath, persistSelection: true))
        {
            Status = "Repositorio nao encontrado. Usando sample-repository.";
            TryOpenRepository(null, persistSelection: false, preserveStatus: true);
        }
    }

    public bool TryOpenRepository(string? repositoryPath, bool persistSelection = false, bool preserveStatus = false)
    {
        var statusToPreserve = preserveStatus ? Status : null;

        if (!TrySaveSelectedNote())
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(repositoryPath) && !Directory.Exists(repositoryPath))
        {
            return false;
        }

        var nextRepository = _noteRepositoryFactory(repositoryPath);
        ReloadRepository(nextRepository, statusToPreserve);

        if (persistSelection && !string.IsNullOrWhiteSpace(repositoryPath))
        {
            _settingsStore.SaveLastRepositoryPath(Path.GetFullPath(repositoryPath));
        }

        if (!preserveStatus)
        {
            Status = $"Repositorio aberto: {RepositoryName}";
        }

        return true;
    }

    private void ReloadRepository(INoteRepository noteRepository, string? status = null)
    {
        _noteRepository = noteRepository;
        RepositoryName = noteRepository.CurrentRepository.Name;
        RepositoryPath = noteRepository.CurrentRepository.RootPath;
        SelectedTag = string.Empty;
        _appliedSearchText = SearchText;
        RefreshTree();
        RefreshTagFilters();
        RefreshTrashItems();

        _selectedNode = null;
        OnPropertyChanged(nameof(SelectedNode));
        SelectedNote = noteRepository.GetNotes().FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(status))
        {
            Status = status;
        }
        else if (SelectedNote is null)
        {
            Status = "Repositorio aberto sem notas Markdown";
        }
    }

    private void RefreshTree()
    {
        Nodes.Clear();

        var hasSearch = !string.IsNullOrWhiteSpace(_appliedSearchText);
        var hasTag = !string.IsNullOrWhiteSpace(SelectedTag);
        var highlightedNoteIds = hasSearch || hasTag
            ? GetMatchedNoteIds(_appliedSearchText, SelectedTag)
            : null;
        var sourceNodes = !hasSearch && !hasTag
            ? _noteRepository.GetTree()
            : GetFilteredTree(_appliedSearchText, SelectedTag, highlightedNoteIds!);

        foreach (var node in sourceNodes)
        {
            Nodes.Add(new RepositoryNodeViewModel(node, highlightedNoteIds));
        }
    }

    private void RefreshTrashItems()
    {
        TrashItems.Clear();

        foreach (var item in _noteRepository.GetTrashItems())
        {
            TrashItems.Add(item);
        }

        SelectedTrashItem = TrashItems.FirstOrDefault();
    }

    private void RefreshTagFilters()
    {
        var tagCounts = _noteRepository
            .GetNotes()
            .SelectMany(note => note.Tags)
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .GroupBy(tag => tag, StringComparer.OrdinalIgnoreCase)
            .Select(group => new
            {
                Name = group.OrderBy(tag => tag, StringComparer.Ordinal).First(),
                Count = group.Count()
            })
            .OrderByDescending(tag => tag.Count)
            .ThenBy(tag => tag.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        TagFilters.Clear();

        foreach (var tag in tagCounts)
        {
            TagFilters.Add(new TagFilterViewModel(
                tag.Name,
                tag.Count,
                string.Equals(tag.Name, SelectedTag, StringComparison.OrdinalIgnoreCase),
                SelectTag));
        }

        OnPropertyChanged(nameof(HasTags));
    }

    private void SelectTag(string tag)
    {
        SelectedTag = string.Equals(SelectedTag, tag, StringComparison.OrdinalIgnoreCase) ? string.Empty : tag;
        RefreshTree();
        UpdateSearchStatus();
    }

    private void ClearTagFilter()
    {
        if (string.IsNullOrWhiteSpace(SelectedTag))
        {
            return;
        }

        SelectedTag = string.Empty;
        RefreshTree();
        UpdateSearchStatus();
    }

    private void ClearSearch()
    {
        _searchDebounceCancellation?.Cancel();
        _appliedSearchText = string.Empty;
        SearchText = string.Empty;
        RefreshTree();
        UpdateSearchStatus();
    }

    private void ScheduleSearchRefresh()
    {
        _searchDebounceCancellation?.Cancel();

        if (_searchDebounceDelay <= TimeSpan.Zero)
        {
            ApplySearchRefresh();
            return;
        }

        SearchFeedback = "Buscando...";
        Status = "Buscando...";

        var cancellation = new CancellationTokenSource();
        _searchDebounceCancellation = cancellation;
        _ = ApplySearchRefreshAfterDelayAsync(cancellation.Token);
    }

    private async Task ApplySearchRefreshAfterDelayAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(_searchDebounceDelay, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            ApplySearchRefresh();
        }
        catch (TaskCanceledException)
        {
        }
    }

    private void ApplySearchRefresh()
    {
        _appliedSearchText = SearchText;
        RefreshTree();
        UpdateSearchStatus();
    }

    private IReadOnlyList<RepositoryNode> GetFilteredTree(string query, string tag, ISet<string> matchedNoteIds)
    {
        var filteredNodes = _noteRepository
            .GetTree()
            .Select(node => FilterNode(node, query, tag, matchedNoteIds))
            .Where(node => node is not null)
            .Cast<RepositoryNode>()
            .ToList();

        return filteredNodes;
    }

    private ISet<string> GetMatchedNoteIds(string query, string tag) =>
        _noteRepository
            .GetNotes()
            .Where(note => MatchesTag(note, tag) && MatchesSearch(note, query))
            .Select(note => note.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static RepositoryNode? FilterNode(RepositoryNode node, string query, string tag, ISet<string> matchedNoteIds)
    {
        if (node.Type == RepositoryNodeType.Note)
        {
            var noteMatches = node.NoteId is not null && matchedNoteIds.Contains(node.NoteId);
            var nodeMatches = string.IsNullOrWhiteSpace(tag)
                && (ContainsIgnoreCase(node.Name, query) || ContainsIgnoreCase(node.Path, query));

            return noteMatches || nodeMatches ? node : null;
        }

        var folderMatches = string.IsNullOrWhiteSpace(tag)
            && (ContainsIgnoreCase(node.Name, query) || ContainsIgnoreCase(node.Path, query));
        if (folderMatches)
        {
            return node;
        }

        var children = node.Children
            .Select(child => FilterNode(child, query, tag, matchedNoteIds))
            .Where(child => child is not null)
            .Cast<RepositoryNode>()
            .ToList();

        if (children.Count == 0)
        {
            return null;
        }

        return new RepositoryNode
        {
            Name = node.Name,
            Type = node.Type,
            NoteId = node.NoteId,
            Path = node.Path,
            Children = children
        };
    }

    private static bool MatchesSearch(NoteItem note, string query) =>
        string.IsNullOrWhiteSpace(query)
        || ContainsIgnoreCase(note.Title, query)
        || ContainsIgnoreCase(Path.GetFileName(note.Path), query)
        || ContainsIgnoreCase(note.Path, query)
        || ContainsIgnoreCase(note.Markdown, query);

    private static bool MatchesTag(NoteItem note, string tag) =>
        string.IsNullOrWhiteSpace(tag)
        || note.Tags.Any(noteTag => string.Equals(noteTag, tag, StringComparison.OrdinalIgnoreCase));

    private int GetSearchResultCount() =>
        _noteRepository.GetNotes().Count(note => MatchesTag(note, SelectedTag) && MatchesSearch(note, _appliedSearchText));

    private void UpdateSearchStatus()
    {
        if (string.IsNullOrWhiteSpace(_appliedSearchText) && string.IsNullOrWhiteSpace(SelectedTag))
        {
            Status = _hasUnsavedChanges ? "Alterado" : "Busca limpa";
            SearchFeedback = "Digite para buscar em titulo, caminho e conteudo";
            _searchResultCount = 0;
            OnPropertyChanged(nameof(HasNoSearchResults));
            return;
        }

        var count = GetSearchResultCount();
        var prefix = string.IsNullOrWhiteSpace(SelectedTag) ? "Busca" : $"Tag {SelectedTag}";
        _searchResultCount = count;
        Status = count == 1 ? $"{prefix}: 1 resultado" : $"{prefix}: {count} resultados";
        SearchFeedback = count == 0
            ? "Nenhum resultado encontrado"
            : count == 1
                ? $"{prefix}: 1 resultado"
                : $"{prefix}: {count} resultados";
        OnPropertyChanged(nameof(HasNoSearchResults));
    }

    private static bool ContainsIgnoreCase(string? value, string query) =>
        !string.IsNullOrWhiteSpace(value)
        && value.Contains(query, StringComparison.OrdinalIgnoreCase);

    private static string GetTemplateNoteName(NoteTemplate template) =>
        template.Id == TechnicalNoteTemplateService.FreeNoteTemplateId
            ? "Nova nota"
            : $"Novo {template.SuggestedType}";

    private static IReadOnlyList<string> ParseTagsText(string value) =>
        value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private async Task<string?> PromptForNameAsync(string title, string message, string initialValue)
    {
        var value = await _textPromptService.PromptAsync(title, message, initialValue);
        if (value is null)
        {
            return null;
        }

        value = value.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            LastErrorMessage = "Nome vazio.";
            Status = "Nome invalido";
            return null;
        }

        return value;
    }

    private string? GetTargetFolderPath()
    {
        if (SelectedNode is null)
        {
            return null;
        }

        if (SelectedNode.Type == RepositoryNodeType.Folder)
        {
            return SelectedNode.Path;
        }

        var directoryName = Path.GetDirectoryName(SelectedNode.Path);
        return string.IsNullOrWhiteSpace(directoryName) ? null : directoryName;
    }

    private string GetPromptNameForSelectedNode(RepositoryNodeViewModel node)
    {
        return node.IsNote
            ? Path.GetFileNameWithoutExtension(node.Name)
            : node.Name;
    }

    private static bool IsPathInside(string path, string candidateParentPath) =>
        path.Equals(candidateParentPath, StringComparison.OrdinalIgnoreCase)
        || path.StartsWith(candidateParentPath + "\\", StringComparison.OrdinalIgnoreCase);

    private void SelectNodeByNoteId(string noteId)
    {
        var node = FindNode(Nodes, candidate => candidate.NoteId == noteId);
        if (node is not null)
        {
            SelectedNode = node;
        }
    }

    private void OpenNoteById(string noteId)
    {
        SelectNodeByNoteId(noteId);
        if (SelectedNote?.Id == noteId)
        {
            return;
        }

        _searchDebounceCancellation?.Cancel();
        _appliedSearchText = string.Empty;
        if (SetProperty(ref _searchText, string.Empty, nameof(SearchText)))
        {
            OnPropertyChanged(nameof(HasSearchText));
        }

        SelectedTag = string.Empty;
        RefreshTree();
        SelectNodeByNoteId(noteId);

        if (SelectedNote?.Id != noteId)
        {
            SelectedNote = _noteRepository.GetNoteById(noteId);
        }
    }

    private void SelectNodeByPath(string path)
    {
        var node = FindNode(Nodes, candidate => candidate.Path == path);
        if (node is not null)
        {
            SelectedNode = node;
        }
    }

    private static RepositoryNodeViewModel? FindNode(
        IEnumerable<RepositoryNodeViewModel> nodes,
        Func<RepositoryNodeViewModel, bool> predicate)
    {
        foreach (var node in nodes)
        {
            if (predicate(node))
            {
                return node;
            }

            var nestedNode = FindNode(node.Children, predicate);
            if (nestedNode is not null)
            {
                return nestedNode;
            }
        }

        return null;
    }

    private bool TrySaveSelectedNote()
    {
        if (SelectedNote is null || !_hasUnsavedChanges)
        {
            Status = "Salvo";
            LastErrorMessage = string.Empty;
            return true;
        }

        try
        {
            Status = "Salvando...";
            LastErrorMessage = string.Empty;
            _noteRepository.SaveNote(SelectedNote);
            _hasUnsavedChanges = false;
            Status = "Salvo";
            OnPropertyChanged(nameof(UpdatedAtText));
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao salvar";
            return false;
        }
    }

    private sealed class NullRepositorySettingsStore : IRepositorySettingsStore
    {
        public string? GetLastRepositoryPath() => null;

        public void SaveLastRepositoryPath(string repositoryPath)
        {
        }
    }
}

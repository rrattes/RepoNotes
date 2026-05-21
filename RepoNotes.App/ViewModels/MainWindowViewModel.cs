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
    private readonly IRepositorySettingsStore _settingsStore;
    private readonly INoteTemplateService _noteTemplateService;
    private readonly Func<string?, INoteRepository> _noteRepositoryFactory;
    private NoteItem? _selectedNote;
    private RepositoryNodeViewModel? _selectedNode;
    private string _repositoryName;
    private string _repositoryPath;
    private string _searchText = string.Empty;
    private string _status = "Salvo";
    private string _lastErrorMessage = string.Empty;
    private bool _hasUnsavedChanges;
    private NoteTemplate _selectedTemplate;

    public MainWindowViewModel(
        INoteRepository noteRepository,
        IFolderPickerService? folderPickerService = null,
        IRepositorySettingsStore? settingsStore = null,
        Func<string?, INoteRepository>? noteRepositoryFactory = null,
        string? initialStatus = null,
        MarkdownPreviewService? markdownPreviewService = null,
        INoteTemplateService? noteTemplateService = null)
    {
        _noteRepository = noteRepository;
        _folderPickerService = folderPickerService ?? new NullFolderPickerService();
        _markdownPreviewService = markdownPreviewService ?? new MarkdownPreviewService();
        _settingsStore = settingsStore ?? new NullRepositorySettingsStore();
        _noteTemplateService = noteTemplateService ?? new TechnicalNoteTemplateService();
        Templates = _noteTemplateService.GetTemplates();
        _selectedTemplate = _noteTemplateService.GetDefaultTemplate();
        _noteRepositoryFactory = noteRepositoryFactory ?? (_ => noteRepository);
        _repositoryName = noteRepository.CurrentRepository.Name;
        _repositoryPath = noteRepository.CurrentRepository.RootPath;
        Nodes = [];
        PreviewBlocks = [];

        NewNoteCommand = new RelayCommand(CreateNewNote);
        NewFromTemplateCommand = new RelayCommand(CreateNewNoteFromSelectedTemplate);
        NewFolderCommand = new RelayCommand(CreateNewFolder);
        OpenFavoritesCommand = new RelayCommand(() => Status = "Favoritos ainda nao implementados no MVP");
        OpenRepositoryCommand = new AsyncRelayCommand(OpenRepositoryAsync);
        OpenSettingsCommand = new RelayCommand(() => Status = "Configuracoes ainda nao implementadas no MVP");
        RenameSelectedItemCommand = new RelayCommand(RenameSelectedItem);
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
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

    public IReadOnlyList<NoteTemplate> Templates { get; }

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

    public ICommand RenameSelectedItemCommand { get; }

    public ICommand DeleteSelectedItemCommand { get; }

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

            RefreshTree();
            UpdateSearchStatus();
        }
    }

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
            OnPropertyChanged(nameof(TagsText));
            UpdatePreviewBlocks();
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

    public string TagsText => SelectedNote is null || SelectedNote.Tags.Count == 0
        ? "Sem tags"
        : string.Join(", ", SelectedNote.Tags);

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
    }

    private void CreateNewNote()
    {
        CreateNewNoteFromTemplate(_noteTemplateService.GetDefaultTemplate(), "Nova nota", "Nota criada");
    }

    private void CreateNewNoteFromSelectedTemplate()
    {
        var noteName = GetTemplateNoteName(SelectedTemplate);
        CreateNewNoteFromTemplate(SelectedTemplate, noteName, "Nota criada por template");
    }

    private void CreateNewNoteFromTemplate(NoteTemplate template, string noteName, string successPrefix)
    {
        if (!TrySaveSelectedNote())
        {
            return;
        }

        try
        {
            var note = _noteRepository.CreateNote(GetTargetFolderPath(), noteName, template);
            RefreshTree();
            SelectNodeByNoteId(note.Id);
            Status = $"{successPrefix}: {note.Path}";
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao criar nota";
        }
    }

    private void CreateNewFolder()
    {
        if (!TrySaveSelectedNote())
        {
            return;
        }

        try
        {
            var folderPath = _noteRepository.CreateFolder(GetTargetFolderPath());
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

    private void RenameSelectedItem()
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

        try
        {
            var wasNote = SelectedNode.IsNote;
            var newPath = _noteRepository.RenameItem(SelectedNode.Path, GetRenameTargetName(SelectedNode));
            RefreshTree();

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
        RefreshTree();

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

        var sourceNodes = string.IsNullOrWhiteSpace(SearchText)
            ? _noteRepository.GetTree()
            : GetFilteredTree(SearchText);

        foreach (var node in sourceNodes)
        {
            Nodes.Add(new RepositoryNodeViewModel(node));
        }
    }

    private IReadOnlyList<RepositoryNode> GetFilteredTree(string query)
    {
        var matchedNoteIds = _noteRepository
            .GetNotes()
            .Where(note => MatchesSearch(note, query))
            .Select(note => note.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var filteredNodes = _noteRepository
            .GetTree()
            .Select(node => FilterNode(node, query, matchedNoteIds))
            .Where(node => node is not null)
            .Cast<RepositoryNode>()
            .ToList();

        return filteredNodes;
    }

    private static RepositoryNode? FilterNode(RepositoryNode node, string query, ISet<string> matchedNoteIds)
    {
        if (node.Type == RepositoryNodeType.Note)
        {
            var noteMatches = node.NoteId is not null && matchedNoteIds.Contains(node.NoteId);
            var nodeMatches = ContainsIgnoreCase(node.Name, query) || ContainsIgnoreCase(node.Path, query);

            return noteMatches || nodeMatches ? node : null;
        }

        var folderMatches = ContainsIgnoreCase(node.Name, query) || ContainsIgnoreCase(node.Path, query);
        if (folderMatches)
        {
            return node;
        }

        var children = node.Children
            .Select(child => FilterNode(child, query, matchedNoteIds))
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
        ContainsIgnoreCase(note.Title, query)
        || ContainsIgnoreCase(Path.GetFileName(note.Path), query)
        || ContainsIgnoreCase(note.Path, query)
        || ContainsIgnoreCase(note.Markdown, query);

    private int GetSearchResultCount() =>
        string.IsNullOrWhiteSpace(SearchText)
            ? _noteRepository.GetNotes().Count
            : _noteRepository.GetNotes().Count(note => MatchesSearch(note, SearchText));

    private void UpdateSearchStatus()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Status = _hasUnsavedChanges ? "Alterado" : "Busca limpa";
            return;
        }

        var count = GetSearchResultCount();
        Status = count == 1 ? "Busca: 1 resultado" : $"Busca: {count} resultados";
    }

    private static bool ContainsIgnoreCase(string? value, string query) =>
        !string.IsNullOrWhiteSpace(value)
        && value.Contains(query, StringComparison.OrdinalIgnoreCase);

    private static string GetTemplateNoteName(NoteTemplate template) =>
        template.Id == TechnicalNoteTemplateService.FreeNoteTemplateId
            ? "Nova nota"
            : $"Novo {template.SuggestedType}";

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

    private string GetRenameTargetName(RepositoryNodeViewModel node)
    {
        var currentName = node.IsNote ? Path.GetFileNameWithoutExtension(node.Name) : node.Name;

        if (node.IsNote
            && SelectedNote is not null
            && node.NoteId == SelectedNote.Id
            && !string.IsNullOrWhiteSpace(Title)
            && !string.Equals(currentName, Title, StringComparison.OrdinalIgnoreCase))
        {
            return Title;
        }

        return $"{currentName} renomeado";
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

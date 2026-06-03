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
    private NoteTabViewModel? _activeTab;
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
    private string _commandPaletteSearchText = string.Empty;
    private CommandPaletteItemViewModel? _selectedCommandPaletteItem;
    private bool _hasUnsavedChanges;
    private bool _isCommandPaletteOpen;
    private bool _isLeftSidebarCollapsed;
    private bool _isRightSidebarCollapsed;
    private DocumentViewMode _documentViewMode = DocumentViewMode.Editor;
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
        OpenTabs = [];
        PreviewBlocks = [];
        InternalLinks = [];
        TrashItems = [];
        TagFilters = [];
        CommandPaletteItems = CreateCommandPaletteItems();
        FilteredCommandPaletteItems = [];

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
        CloseTabCommand = new RelayCommand(CloseActiveTab, () => ActiveTab is not null);
        OpenExplorerItemCommand = new ParameterizedRelayCommand<RepositoryNodeViewModel>(OpenExplorerItem);
        RenameExplorerItemCommand = new ParameterizedAsyncRelayCommand<RepositoryNodeViewModel>(RenameExplorerItemAsync);
        MoveExplorerItemToTrashCommand = new ParameterizedRelayCommand<RepositoryNodeViewModel>(MoveExplorerItemToTrash);
        NewNoteAtExplorerItemCommand = new ParameterizedAsyncRelayCommand<RepositoryNodeViewModel>(CreateNewNoteAtExplorerItemAsync);
        NewFolderAtExplorerItemCommand = new ParameterizedAsyncRelayCommand<RepositoryNodeViewModel>(CreateNewFolderAtExplorerItemAsync);
        CloseTabItemCommand = new ParameterizedRelayCommand<NoteTabViewModel>(CloseTabItem);
        CloseOtherTabsCommand = new ParameterizedRelayCommand<NoteTabViewModel>(CloseOtherTabs);
        CloseAllTabsCommand = new RelayCommand(CloseAllTabs, () => OpenTabs.Count > 0);
        RestoreTrashItemCommand = new ParameterizedRelayCommand<TrashItem>(RestoreTrashItem);
        DeleteTrashItemPermanentlyCommand = new ParameterizedRelayCommand<TrashItem>(DeleteTrashItemPermanently);
        ShowEditorCommand = new RelayCommand(ShowEditor);
        ShowPreviewCommand = new RelayCommand(ShowPreview);
        ShowSplitCommand = new RelayCommand(ShowSplit);
        ToggleLeftSidebarCommand = new RelayCommand(ToggleLeftSidebar);
        ToggleRightSidebarCommand = new RelayCommand(ToggleRightSidebar);
        OpenCommandPaletteCommand = new RelayCommand(OpenCommandPalette);
        CloseCommandPaletteCommand = new RelayCommand(CloseCommandPalette);
        ExecuteSelectedCommandPaletteItemCommand = new RelayCommand(ExecuteSelectedCommandPaletteItem);

        ReloadRepository(noteRepository, initialStatus);
        RefreshCommandPaletteFilter();
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

    public ObservableCollection<NoteTabViewModel> OpenTabs { get; }

    public ObservableCollection<MarkdownPreviewBlock> PreviewBlocks { get; }

    public ObservableCollection<InternalLinkViewModel> InternalLinks { get; }

    public ObservableCollection<TrashItem> TrashItems { get; }

    public ObservableCollection<TagFilterViewModel> TagFilters { get; }

    public IReadOnlyList<NoteTemplate> Templates { get; }

    public IReadOnlyList<CommandPaletteItemViewModel> CommandPaletteItems { get; }

    public ObservableCollection<CommandPaletteItemViewModel> FilteredCommandPaletteItems { get; }

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

    public ICommand CloseTabCommand { get; }

    public ICommand OpenExplorerItemCommand { get; }

    public ICommand RenameExplorerItemCommand { get; }

    public ICommand MoveExplorerItemToTrashCommand { get; }

    public ICommand NewNoteAtExplorerItemCommand { get; }

    public ICommand NewFolderAtExplorerItemCommand { get; }

    public ICommand CloseTabItemCommand { get; }

    public ICommand CloseOtherTabsCommand { get; }

    public ICommand CloseAllTabsCommand { get; }

    public ICommand RestoreTrashItemCommand { get; }

    public ICommand DeleteTrashItemPermanentlyCommand { get; }

    public ICommand ShowEditorCommand { get; }

    public ICommand ShowPreviewCommand { get; }

    public ICommand ShowSplitCommand { get; }

    public ICommand ToggleLeftSidebarCommand { get; }

    public ICommand ToggleRightSidebarCommand { get; }

    public ICommand OpenCommandPaletteCommand { get; }

    public ICommand CloseCommandPaletteCommand { get; }

    public ICommand ExecuteSelectedCommandPaletteItemCommand { get; }

    public DocumentViewMode DocumentViewMode => _documentViewMode;

    public bool IsEditorMode => _documentViewMode == DocumentViewMode.Editor;

    public bool IsPreviewMode => _documentViewMode == DocumentViewMode.Preview;

    public bool IsSplitMode => _documentViewMode == DocumentViewMode.Split;

    public bool HasEditorVisible => _documentViewMode is DocumentViewMode.Editor or DocumentViewMode.Split;

    public bool HasPreviewVisible => _documentViewMode is DocumentViewMode.Preview or DocumentViewMode.Split;

    public bool HasOpenTabs => OpenTabs.Count > 0;

    public bool IsLeftSidebarCollapsed => _isLeftSidebarCollapsed;

    public bool IsLeftSidebarExpanded => !_isLeftSidebarCollapsed;

    public bool IsRightSidebarCollapsed => _isRightSidebarCollapsed;

    public bool IsRightSidebarExpanded => !_isRightSidebarCollapsed;

    public double LeftSidebarWidth => IsLeftSidebarCollapsed ? 42 : 252;

    public double RightSidebarWidth => IsRightSidebarCollapsed ? 42 : 326;

    public bool IsCommandPaletteOpen
    {
        get => _isCommandPaletteOpen;
        private set => SetProperty(ref _isCommandPaletteOpen, value);
    }

    public string CommandPaletteSearchText
    {
        get => _commandPaletteSearchText;
        set
        {
            if (!SetProperty(ref _commandPaletteSearchText, value))
            {
                return;
            }

            RefreshCommandPaletteFilter();
        }
    }

    public CommandPaletteItemViewModel? SelectedCommandPaletteItem
    {
        get => _selectedCommandPaletteItem;
        private set
        {
            if (ReferenceEquals(_selectedCommandPaletteItem, value))
            {
                return;
            }

            if (_selectedCommandPaletteItem is not null)
            {
                _selectedCommandPaletteItem.IsSelected = false;
            }

            _selectedCommandPaletteItem = value;

            if (_selectedCommandPaletteItem is not null)
            {
                _selectedCommandPaletteItem.IsSelected = true;
            }

            OnPropertyChanged();
        }
    }

    public bool HasCommandPaletteResults => FilteredCommandPaletteItems.Count > 0;

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

            if (!SetProperty(ref _selectedNode, value) || value?.NoteId is null)
            {
                return;
            }

            OpenNoteInTab(value.NoteId);
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
        }
    }

    public NoteTabViewModel? ActiveTab
    {
        get => _activeTab;
        private set
        {
            if (ReferenceEquals(_activeTab, value))
            {
                return;
            }

            if (_activeTab is not null)
            {
                _activeTab.IsActive = false;
            }

            _activeTab = value;
            if (_activeTab is not null)
            {
                _activeTab.IsActive = true;
            }

            OnPropertyChanged();
            OnPropertyChanged(nameof(HasOpenTabs));
            SelectedNote = _activeTab?.Note;
            _hasUnsavedChanges = _activeTab?.IsDirty ?? false;
            LastErrorMessage = _activeTab?.LastErrorMessage ?? string.Empty;
            Status = _activeTab?.Status ?? "Nenhuma aba aberta";
            (SaveNoteCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CloseTabCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CloseAllTabsCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
        private set
        {
            if (SetProperty(ref _status, value) && ActiveTab is not null)
            {
                ActiveTab.Status = value;
            }
        }
    }

    public string LastErrorMessage
    {
        get => _lastErrorMessage;
        private set
        {
            if (SetProperty(ref _lastErrorMessage, value) && ActiveTab is not null)
            {
                ActiveTab.LastErrorMessage = value;
            }
        }
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
        if (ActiveTab is not null)
        {
            ActiveTab.IsDirty = true;
            ActiveTab.RefreshFromNote();
        }

        OnPropertyChanged(nameof(UpdatedAtText));
    }

    private void SaveSelectedNote()
    {
        _ = TrySaveSelectedNote();
    }

    private void OpenNoteInTab(string noteId)
    {
        var existingTab = OpenTabs.FirstOrDefault(tab => tab.NoteId == noteId);
        if (existingTab is not null)
        {
            ActiveTab = existingTab;
            return;
        }

        var note = _noteRepository.GetNoteById(noteId);
        if (note is null)
        {
            Status = "Nota nao encontrada";
            return;
        }

        var tab = new NoteTabViewModel(note, ActivateTab, CloseTab);
        OpenTabs.Add(tab);
        OnPropertyChanged(nameof(HasOpenTabs));
        ActiveTab = tab;
    }

    private void ActivateTab(NoteTabViewModel tab)
    {
        if (!OpenTabs.Contains(tab))
        {
            return;
        }

        ActiveTab = tab;
        SelectNodeByNoteIdWithoutOpening(tab.NoteId);
    }

    private void CloseActiveTab()
    {
        if (ActiveTab is not null)
        {
            CloseTab(ActiveTab);
        }
    }

    private void CloseTab(NoteTabViewModel tab)
    {
        if (!OpenTabs.Contains(tab))
        {
            return;
        }

        if (!TrySaveTab(tab))
        {
            ActiveTab = tab;
            return;
        }

        var index = OpenTabs.IndexOf(tab);
        var wasActive = ReferenceEquals(tab, ActiveTab);
        OpenTabs.Remove(tab);
        OnPropertyChanged(nameof(HasOpenTabs));

        if (!wasActive)
        {
            return;
        }

        if (OpenTabs.Count == 0)
        {
            ActiveTab = null;
            _selectedNode = null;
            OnPropertyChanged(nameof(SelectedNode));
            return;
        }

        ActiveTab = OpenTabs[Math.Min(index, OpenTabs.Count - 1)];
        SelectNodeByNoteIdWithoutOpening(ActiveTab.NoteId);
    }

    private void CloseTabItem(NoteTabViewModel? tab)
    {
        if (tab is not null)
        {
            CloseTab(tab);
        }
    }

    private void CloseOtherTabs(NoteTabViewModel? tab)
    {
        if (tab is null || !OpenTabs.Contains(tab))
        {
            return;
        }

        foreach (var openTab in OpenTabs.Where(openTab => !ReferenceEquals(openTab, tab)).ToList())
        {
            CloseTab(openTab);
        }

        if (OpenTabs.Contains(tab))
        {
            ActiveTab = tab;
            SelectNodeByNoteIdWithoutOpening(tab.NoteId);
            Status = "Outras abas fechadas";
        }
    }

    private void CloseAllTabs()
    {
        foreach (var tab in OpenTabs.ToList())
        {
            CloseTab(tab);
        }

        if (OpenTabs.Count == 0)
        {
            Status = "Todas as abas fechadas";
        }
    }

    private void ShowEditor()
    {
        SetDocumentViewMode(DocumentViewMode.Editor);
    }

    private void ShowPreview()
    {
        SetDocumentViewMode(DocumentViewMode.Preview);
    }

    private void ShowSplit()
    {
        SetDocumentViewMode(DocumentViewMode.Split);
    }

    private void ToggleLeftSidebar()
    {
        _isLeftSidebarCollapsed = !_isLeftSidebarCollapsed;
        OnPropertyChanged(nameof(IsLeftSidebarCollapsed));
        OnPropertyChanged(nameof(IsLeftSidebarExpanded));
        OnPropertyChanged(nameof(LeftSidebarWidth));
    }

    private void ToggleRightSidebar()
    {
        _isRightSidebarCollapsed = !_isRightSidebarCollapsed;
        OnPropertyChanged(nameof(IsRightSidebarCollapsed));
        OnPropertyChanged(nameof(IsRightSidebarExpanded));
        OnPropertyChanged(nameof(RightSidebarWidth));
    }

    private void OpenCommandPalette()
    {
        CommandPaletteSearchText = string.Empty;
        IsCommandPaletteOpen = true;
        RefreshCommandPaletteFilter();
    }

    private void CloseCommandPalette()
    {
        IsCommandPaletteOpen = false;
        CommandPaletteSearchText = string.Empty;
    }

    private void ExecuteSelectedCommandPaletteItem()
    {
        if (SelectedCommandPaletteItem is not null)
        {
            ExecuteCommandPaletteItem(SelectedCommandPaletteItem);
        }
    }

    public bool ExecuteCommandPaletteItem(CommandPaletteItemViewModel item)
    {
        if (item.RequiresEditor)
        {
            return false;
        }

        switch (item.ActionKind)
        {
            case CommandPaletteActionKind.ShowEditor:
                ShowEditorCommand.Execute(null);
                break;
            case CommandPaletteActionKind.ShowPreview:
                ShowPreviewCommand.Execute(null);
                break;
            case CommandPaletteActionKind.ShowSplit:
                ShowSplitCommand.Execute(null);
                break;
            case CommandPaletteActionKind.Save:
                SaveNoteCommand.Execute(null);
                break;
            case CommandPaletteActionKind.NewNote:
                NewNoteCommand.Execute(null);
                break;
            case CommandPaletteActionKind.NewFolder:
                NewFolderCommand.Execute(null);
                break;
            case CommandPaletteActionKind.Rename:
                RenameSelectedItemCommand.Execute(null);
                break;
            case CommandPaletteActionKind.MoveToTrash:
                DeleteSelectedItemCommand.Execute(null);
                break;
            default:
                return false;
        }

        CloseCommandPalette();
        return true;
    }

    public void SelectNextCommandPaletteItem()
    {
        if (FilteredCommandPaletteItems.Count == 0)
        {
            SelectedCommandPaletteItem = null;
            return;
        }

        var index = SelectedCommandPaletteItem is null
            ? -1
            : FilteredCommandPaletteItems.IndexOf(SelectedCommandPaletteItem);
        SelectedCommandPaletteItem = FilteredCommandPaletteItems[(index + 1) % FilteredCommandPaletteItems.Count];
    }

    public void SelectPreviousCommandPaletteItem()
    {
        if (FilteredCommandPaletteItems.Count == 0)
        {
            SelectedCommandPaletteItem = null;
            return;
        }

        var index = SelectedCommandPaletteItem is null
            ? 0
            : FilteredCommandPaletteItems.IndexOf(SelectedCommandPaletteItem);
        if (index <= 0)
        {
            index = FilteredCommandPaletteItems.Count;
        }

        SelectedCommandPaletteItem = FilteredCommandPaletteItems[index - 1];
    }

    private void RefreshCommandPaletteFilter()
    {
        var selectedId = SelectedCommandPaletteItem?.Id;
        FilteredCommandPaletteItems.Clear();

        foreach (var item in CommandPaletteItems.Where(item => item.Matches(CommandPaletteSearchText)).Take(12))
        {
            FilteredCommandPaletteItems.Add(item);
        }

        SelectedCommandPaletteItem = FilteredCommandPaletteItems.FirstOrDefault(item => item.Id == selectedId)
            ?? FilteredCommandPaletteItems.FirstOrDefault();
        OnPropertyChanged(nameof(HasCommandPaletteResults));
    }

    private void SetDocumentViewMode(DocumentViewMode mode)
    {
        if (_documentViewMode == mode)
        {
            return;
        }

        _documentViewMode = mode;
        OnPropertyChanged(nameof(DocumentViewMode));
        OnPropertyChanged(nameof(IsEditorMode));
        OnPropertyChanged(nameof(IsPreviewMode));
        OnPropertyChanged(nameof(IsSplitMode));
        OnPropertyChanged(nameof(HasEditorVisible));
        OnPropertyChanged(nameof(HasPreviewVisible));
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

        OpenNoteById(link.NoteId);
        Status = $"Link interno aberto: {link.DisplayText}";
    }

    private void OpenExplorerItem(RepositoryNodeViewModel? node)
    {
        if (node is null)
        {
            return;
        }

        SelectedNode = node;
        Status = node.IsNote ? $"Nota aberta: {node.Path}" : $"Pasta selecionada: {node.Path}";
    }

    private async Task RenameExplorerItemAsync(RepositoryNodeViewModel? node)
    {
        if (node is null)
        {
            return;
        }

        SelectedNode = node;
        await RenameSelectedItemAsync();
    }

    private void MoveExplorerItemToTrash(RepositoryNodeViewModel? node)
    {
        if (node is null)
        {
            return;
        }

        SelectedNode = node;
        DeleteSelectedItem();
    }

    private async Task CreateNewNoteAtExplorerItemAsync(RepositoryNodeViewModel? node)
    {
        if (node is not null)
        {
            SelectedNode = node;
        }

        await CreateNewNoteAsync();
    }

    private async Task CreateNewFolderAtExplorerItemAsync(RepositoryNodeViewModel? node)
    {
        if (node is not null)
        {
            SelectedNode = node;
        }

        await CreateNewFolderAsync();
    }

    private void RestoreTrashItem(TrashItem? item)
    {
        if (item is null)
        {
            return;
        }

        SelectedTrashItem = item;
        RestoreFromTrash();
    }

    private void DeleteTrashItemPermanently(TrashItem? item)
    {
        if (item is null)
        {
            return;
        }

        SelectedTrashItem = item;
        DeletePermanently();
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

        var defaultName = GetPromptNameForSelectedNode(SelectedNode);
        var newName = await PromptForNameAsync("Renomear item", "Novo nome", defaultName);
        if (newName is null)
        {
            Status = "Renomeacao cancelada";
            return;
        }

        try
        {
            var oldPath = SelectedNode.Path;
            if (!TrySaveOpenTabsInsidePath(oldPath))
            {
                return;
            }

            var wasNote = SelectedNode.IsNote;
            var newPath = _noteRepository.RenameItem(oldPath, newName);
            RefreshTree();
            RefreshTagFilters();
            RefreshOpenTabsAfterRepositoryChange(oldPath, newPath);

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

        var deletedPath = SelectedNode.Path;

        try
        {
            if (!TrySaveOpenTabsInsidePath(deletedPath))
            {
                return;
            }

            _noteRepository.MoveItemToTrash(deletedPath);
            RefreshTree();
            RefreshTagFilters();
            RefreshTrashItems();
            CloseTabsInsidePath(deletedPath);

            if (ActiveTab is null)
            {
                var nextNote = _noteRepository.GetNotes().FirstOrDefault();
                if (nextNote is not null)
                {
                    SelectNodeByNoteId(nextNote.Id);
                }
                else
                {
                    _selectedNode = null;
                    OnPropertyChanged(nameof(SelectedNode));
                }
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
        OpenTabs.Clear();
        OnPropertyChanged(nameof(HasOpenTabs));
        ActiveTab = null;

        var firstNote = noteRepository.GetNotes().FirstOrDefault();
        if (firstNote is not null)
        {
            OpenNoteInTab(firstNote.Id);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            Status = status;
        }
        else if (firstNote is null)
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

    private static IReadOnlyList<CommandPaletteItemViewModel> CreateCommandPaletteItems() =>
    [
        new("show-editor", "Show Editor", "Switch central workspace to Markdown editor", "Editor", CommandPaletteActionKind.ShowEditor),
        new("show-preview", "Show Preview", "Switch central workspace to rendered preview", "Preview", CommandPaletteActionKind.ShowPreview),
        new("show-split", "Show Split", "Show Markdown editor and preview side by side", "Split", CommandPaletteActionKind.ShowSplit),
        new("bold", "Bold", "Apply Markdown bold formatting", "Ctrl+B", CommandPaletteActionKind.Bold, requiresEditor: true),
        new("italic", "Italic", "Apply Markdown italic formatting", "Ctrl+I", CommandPaletteActionKind.Italic, requiresEditor: true),
        new("heading-1", "Heading 1", "Apply H1 heading formatting", "Ctrl+Alt+1", CommandPaletteActionKind.Heading1, requiresEditor: true),
        new("heading-2", "Heading 2", "Apply H2 heading formatting", "Ctrl+Alt+2", CommandPaletteActionKind.Heading2, requiresEditor: true),
        new("heading-3", "Heading 3", "Apply H3 heading formatting", "Ctrl+Alt+3", CommandPaletteActionKind.Heading3, requiresEditor: true),
        new("list", "List", "Apply Markdown list formatting", "Ctrl+Shift+7", CommandPaletteActionKind.List, requiresEditor: true),
        new("checklist", "Checklist", "Apply Markdown checklist formatting", "Ctrl+Shift+8", CommandPaletteActionKind.Checklist, requiresEditor: true),
        new("quote", "Quote", "Apply Markdown quote formatting", "Ctrl+Shift+Q", CommandPaletteActionKind.Quote, requiresEditor: true),
        new("code", "Code", "Apply inline or block code formatting", "Ctrl+`", CommandPaletteActionKind.Code, requiresEditor: true),
        new("link", "Link", "Insert Markdown link formatting", "Ctrl+K", CommandPaletteActionKind.Link, requiresEditor: true),
        new("insert-table", "Insert Table", "Insert a basic 3-column Markdown table", "", CommandPaletteActionKind.InsertTable, requiresEditor: true),
        new("insert-code-block", "Insert Code Block", "Insert a fenced Markdown code block", "", CommandPaletteActionKind.InsertCodeBlock, requiresEditor: true),
        new("insert-callout", "Insert Callout", "Insert a Markdown callout/admonition block", "", CommandPaletteActionKind.InsertCallout, requiresEditor: true),
        new("save", "Save", "Save the active note", "Ctrl+S", CommandPaletteActionKind.Save),
        new("new-note", "New Note", "Create a new Markdown note", "", CommandPaletteActionKind.NewNote),
        new("new-folder", "New Folder", "Create a new folder", "", CommandPaletteActionKind.NewFolder),
        new("rename", "Rename", "Rename the selected note or folder", "", CommandPaletteActionKind.Rename),
        new("move-to-trash", "Move to Trash", "Move the selected note or folder to trash", "", CommandPaletteActionKind.MoveToTrash)
    ];

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

    private static bool IsPathInside(string path, string candidateParentPath)
    {
        var normalizedPath = NormalizeForComparison(path);
        var normalizedParentPath = NormalizeForComparison(candidateParentPath);

        return normalizedPath.Equals(normalizedParentPath, StringComparison.OrdinalIgnoreCase)
            || normalizedPath.StartsWith(normalizedParentPath + "\\", StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeForComparison(string value) => value.Replace('/', '\\').Trim('\\');

    private bool TrySaveOpenTabsInsidePath(string path)
    {
        foreach (var tab in OpenTabs.Where(tab => IsPathInside(tab.Path, path)).ToList())
        {
            if (!TrySaveTab(tab))
            {
                ActiveTab = tab;
                SelectNodeByNoteIdWithoutOpening(tab.NoteId);
                return false;
            }
        }

        return true;
    }

    private void CloseTabsInsidePath(string path)
    {
        var tabsToClose = OpenTabs.Where(tab => IsPathInside(tab.Path, path)).ToList();
        foreach (var tab in tabsToClose)
        {
            var wasActive = ReferenceEquals(tab, ActiveTab);
            OpenTabs.Remove(tab);
            if (wasActive)
            {
                ActiveTab = null;
            }
        }

        OnPropertyChanged(nameof(HasOpenTabs));

        if (ActiveTab is null && OpenTabs.Count > 0)
        {
            ActiveTab = OpenTabs[0];
            SelectNodeByNoteIdWithoutOpening(ActiveTab.NoteId);
        }
        else if (ActiveTab is null)
        {
            _selectedNode = null;
            OnPropertyChanged(nameof(SelectedNode));
        }
    }

    private void RefreshOpenTabsAfterRepositoryChange(string oldPath, string newPath)
    {
        foreach (var tab in OpenTabs.ToList())
        {
            var nextPath = IsPathInside(tab.Path, oldPath)
                ? newPath + tab.Path[oldPath.Length..]
                : tab.Path;
            var note = _noteRepository.GetNoteById(nextPath);
            if (note is not null)
            {
                tab.UpdateNote(note);
            }
        }

        if (ActiveTab is not null)
        {
            SelectedNote = ActiveTab.Note;
        }
    }

    private void SelectNodeByNoteId(string noteId)
    {
        var node = FindNode(Nodes, candidate => candidate.NoteId == noteId);
        if (node is not null)
        {
            SelectedNode = node;
        }
    }

    private void SelectNodeByNoteIdWithoutOpening(string noteId)
    {
        var node = FindNode(Nodes, candidate => candidate.NoteId == noteId);
        if (node is not null)
        {
            _selectedNode = node;
            OnPropertyChanged(nameof(SelectedNode));
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
            OpenNoteInTab(noteId);
            SelectNodeByNoteIdWithoutOpening(noteId);
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
        return ActiveTab is null || TrySaveTab(ActiveTab);
    }

    private bool TrySaveTab(NoteTabViewModel tab)
    {
        if (!tab.IsDirty)
        {
            if (ReferenceEquals(tab, ActiveTab))
            {
                Status = "Salvo";
                LastErrorMessage = string.Empty;
            }

            tab.Status = "Salvo";
            tab.LastErrorMessage = string.Empty;
            return true;
        }

        try
        {
            if (ReferenceEquals(tab, ActiveTab))
            {
                Status = "Salvando...";
                LastErrorMessage = string.Empty;
            }

            tab.Status = "Salvando...";
            tab.LastErrorMessage = string.Empty;
            _noteRepository.SaveNote(tab.Note);
            tab.IsDirty = false;
            tab.Status = "Salvo";
            tab.LastErrorMessage = string.Empty;
            tab.RefreshFromNote();

            if (ReferenceEquals(tab, ActiveTab))
            {
                _hasUnsavedChanges = false;
                Status = "Salvo";
                LastErrorMessage = string.Empty;
                OnPropertyChanged(nameof(UpdatedAtText));
            }

            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            tab.LastErrorMessage = ex.Message;
            tab.Status = "Erro ao salvar";
            if (ReferenceEquals(tab, ActiveTab))
            {
                LastErrorMessage = ex.Message;
                Status = "Erro ao salvar";
            }

            return false;
        }
    }

    public (string newText, int newSelStart, int newSelEnd) ApplyMarkdownFormat(
        string text, int selStart, int selEnd, string formatType)
    {
        selStart = Math.Clamp(selStart, 0, text.Length);
        selEnd = Math.Clamp(selEnd, selStart, text.Length);
        var selection = text[selStart..selEnd];

        return formatType switch
        {
            "bold" => WrapInline(text, selStart, selEnd, selection, "**"),
            "italic" => WrapInline(text, selStart, selEnd, selection, "*"),
            "h1" => ApplyHeadingFormat(text, selStart, selEnd, "# "),
            "h2" => ApplyHeadingFormat(text, selStart, selEnd, "## "),
            "h3" => ApplyHeadingFormat(text, selStart, selEnd, "### "),
            "list" => ApplyLinePrefix(text, selStart, selEnd, "- "),
            "checklist" => ApplyLinePrefix(text, selStart, selEnd, "- [ ] "),
            "quote" => ApplyLinePrefix(text, selStart, selEnd, "> "),
            "link" => ApplyLink(text, selStart, selEnd, selection),
            "code" => ApplyCode(text, selStart, selEnd, selection),
            _ => (text, selStart, selEnd)
        };
    }

    public (string newText, int newSelStart, int newSelEnd) ApplyMarkdownInsertion(
        string text, int selStart, int selEnd, string insertionType)
    {
        selStart = Math.Clamp(selStart, 0, text.Length);
        selEnd = Math.Clamp(selEnd, selStart, text.Length);
        var selection = text[selStart..selEnd];

        return insertionType switch
        {
            "table" => InsertBlock(text, selStart, selEnd, """
            | Column 1 | Column 2 | Column 3 |
            |---|---|---|
            |  |  |  |
            |  |  |  |
            """),
            "code-block" => InsertCodeBlock(text, selStart, selEnd, selection),
            "callout" => InsertBlock(text, selStart, selEnd, """
            > [!NOTE]
            > Callout content
            """),
            _ => (text, selStart, selEnd)
        };
    }

    private static (string, int, int) WrapInline(string text, int selStart, int selEnd, string selection, string marker)
    {
        if (selection.Length > 0)
        {
            var newText = text[..selStart] + marker + selection + marker + text[selEnd..];
            return (newText, selStart + marker.Length, selEnd + marker.Length);
        }
        else
        {
            var newText = text[..selStart] + marker + marker + text[selStart..];
            return (newText, selStart + marker.Length, selStart + marker.Length);
        }
    }

    private static (string, int, int) ApplyHeadingFormat(string text, int selStart, int selEnd, string prefix)
    {
        var lineStart = selStart > 0 ? text.LastIndexOf('\n', selStart - 1) + 1 : 0;
        var lineFromStart = text[lineStart..];

        string[] headingPrefixes = ["### ", "## ", "# "];
        var currentPrefix = headingPrefixes.FirstOrDefault(p => lineFromStart.StartsWith(p, StringComparison.Ordinal));

        if (currentPrefix is null)
        {
            var newText = text[..lineStart] + prefix + text[lineStart..];
            return (newText, selStart + prefix.Length, selEnd + prefix.Length);
        }

        var contentStart = lineStart + currentPrefix.Length;

        if (currentPrefix == prefix)
        {
            var newText = text[..lineStart] + text[contentStart..];
            return (newText,
                    Math.Max(lineStart, selStart - currentPrefix.Length),
                    Math.Max(lineStart, selEnd - currentPrefix.Length));
        }
        else
        {
            var newText = text[..lineStart] + prefix + text[contentStart..];
            var diff = prefix.Length - currentPrefix.Length;
            return (newText, selStart + diff, selEnd + diff);
        }
    }

    private static (string, int, int) ApplyLinePrefix(string text, int selStart, int selEnd, string prefix)
    {
        var lineStart = selStart > 0 ? text.LastIndexOf('\n', selStart - 1) + 1 : 0;

        if (text[lineStart..].StartsWith(prefix, StringComparison.Ordinal))
        {
            var contentStart = lineStart + prefix.Length;
            var newText = text[..lineStart] + text[contentStart..];
            return (newText,
                    Math.Max(lineStart, selStart - prefix.Length),
                    Math.Max(lineStart, selEnd - prefix.Length));
        }
        else
        {
            var newText = text[..lineStart] + prefix + text[lineStart..];
            return (newText, selStart + prefix.Length, selEnd + prefix.Length);
        }
    }

    private static (string, int, int) ApplyLink(string text, int selStart, int selEnd, string selection)
    {
        var linkText = selection.Length > 0 ? selection : "texto";
        var insert = $"[{linkText}](url)";
        var newText = text[..selStart] + insert + text[selEnd..];
        var urlStart = selStart + linkText.Length + 3;
        return (newText, urlStart, urlStart + 3);
    }

    private static (string, int, int) ApplyCode(string text, int selStart, int selEnd, string selection)
    {
        if (selection.Length > 0 && selection.Contains('\n'))
        {
            var newText = text[..selStart] + "```\n" + selection + "\n```" + text[selEnd..];
            return (newText, selStart + 4, selEnd + 4);
        }
        else
        {
            var newText = text[..selStart] + "`" + selection + "`" + text[selEnd..];
            return (newText, selStart + 1, selEnd + 1);
        }
    }

    private static (string, int, int) InsertBlock(string text, int selStart, int selEnd, string block)
    {
        var prefix = selStart > 0 && text[selStart - 1] != '\n' ? Environment.NewLine : string.Empty;
        var suffix = selEnd < text.Length && text[selEnd] != '\n' ? Environment.NewLine : string.Empty;
        var insert = prefix + block + suffix;
        var newText = text[..selStart] + insert + text[selEnd..];
        var cursor = selStart + insert.Length;
        return (newText, cursor, cursor);
    }

    private static (string, int, int) InsertCodeBlock(string text, int selStart, int selEnd, string selection)
    {
        var code = selection.Length > 0 ? selection : "code here";
        var block = $"```text{Environment.NewLine}{code}{Environment.NewLine}```";
        var prefixLength = selStart > 0 && text[selStart - 1] != '\n' ? Environment.NewLine.Length : 0;
        var (newText, _, _) = InsertBlock(text, selStart, selEnd, block);
        var codeStart = selStart + prefixLength + "```text".Length + Environment.NewLine.Length;
        return (newText, codeStart, codeStart + code.Length);
    }

    private sealed class NullRepositorySettingsStore : IRepositorySettingsStore
    {
        public string? GetLastRepositoryPath() => null;

        public void SaveLastRepositoryPath(string repositoryPath)
        {
        }
    }
}

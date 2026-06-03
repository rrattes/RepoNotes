using RepoNotes.App.Services;
using RepoNotes.App.ViewModels;
using RepoNotes.Core.Models;
using RepoNotes.Core.Services;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelTabsTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public MainWindowViewModelTabsTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRepositoryPath);
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "A.md"), """
        ---
        title: Alpha
        type: note
        tags: [one]
        status: draft
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Alpha

        Link para [[Beta]].
        """);
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "B.md"), """
        ---
        title: Beta
        type: note
        tags: [two]
        status: draft
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Beta
        """);
    }

    [Fact]
    public void StartsWithFirstNoteOpenInEditorTab()
    {
        var viewModel = CreateViewModel();

        Assert.Single(viewModel.OpenTabs);
        Assert.True(viewModel.HasOpenTabs);
        Assert.Equal("Alpha", viewModel.ActiveTab?.Title);
        Assert.Equal(DocumentViewMode.Editor, viewModel.DocumentViewMode);
        Assert.True(viewModel.IsEditorMode);
        Assert.True(viewModel.HasEditorVisible);
        Assert.False(viewModel.HasPreviewVisible);
    }

    [Fact]
    public void DocumentViewModeCanSwitchToPreview()
    {
        var viewModel = CreateViewModel();

        viewModel.ShowPreviewCommand.Execute(null);

        Assert.Equal(DocumentViewMode.Preview, viewModel.DocumentViewMode);
        Assert.False(viewModel.IsEditorMode);
        Assert.True(viewModel.IsPreviewMode);
        Assert.False(viewModel.IsSplitMode);
        Assert.False(viewModel.HasEditorVisible);
        Assert.True(viewModel.HasPreviewVisible);
    }

    [Fact]
    public void DocumentViewModeCanSwitchToSplit()
    {
        var viewModel = CreateViewModel();

        viewModel.ShowSplitCommand.Execute(null);

        Assert.Equal(DocumentViewMode.Split, viewModel.DocumentViewMode);
        Assert.False(viewModel.IsEditorMode);
        Assert.False(viewModel.IsPreviewMode);
        Assert.True(viewModel.IsSplitMode);
        Assert.True(viewModel.HasEditorVisible);
        Assert.True(viewModel.HasPreviewVisible);
    }

    [Fact]
    public void SplitPresetStartsBalanced()
    {
        var viewModel = CreateViewModel();

        Assert.Equal("1*,10,1*", viewModel.SplitColumnDefinitions);
        Assert.True(viewModel.IsSplitPreset50);
        Assert.False(viewModel.IsSplitPreset60);
        Assert.False(viewModel.IsSplitPreset70);
    }

    [Fact]
    public void SplitPresetCommandsUpdateColumnDefinitions()
    {
        var viewModel = CreateViewModel();

        viewModel.SetSplitPreset60Command.Execute(null);

        Assert.Equal("3*,10,2*", viewModel.SplitColumnDefinitions);
        Assert.False(viewModel.IsSplitPreset50);
        Assert.True(viewModel.IsSplitPreset60);
        Assert.False(viewModel.IsSplitPreset70);

        viewModel.SetSplitPreset70Command.Execute(null);

        Assert.Equal("7*,10,3*", viewModel.SplitColumnDefinitions);
        Assert.False(viewModel.IsSplitPreset50);
        Assert.False(viewModel.IsSplitPreset60);
        Assert.True(viewModel.IsSplitPreset70);

        viewModel.SetSplitPreset50Command.Execute(null);

        Assert.Equal("1*,10,1*", viewModel.SplitColumnDefinitions);
        Assert.True(viewModel.IsSplitPreset50);
        Assert.False(viewModel.IsSplitPreset60);
        Assert.False(viewModel.IsSplitPreset70);
    }

    [Fact]
    public void SplitPresetSurvivesDocumentModeChanges()
    {
        var viewModel = CreateViewModel();
        viewModel.ShowSplitCommand.Execute(null);
        viewModel.SetSplitPreset70Command.Execute(null);

        viewModel.ShowEditorCommand.Execute(null);
        viewModel.ShowSplitCommand.Execute(null);

        Assert.Equal(DocumentViewMode.Split, viewModel.DocumentViewMode);
        Assert.Equal("7*,10,3*", viewModel.SplitColumnDefinitions);
        Assert.True(viewModel.IsSplitPreset70);
    }

    [Fact]
    public void SidePanelsStartExpanded()
    {
        var viewModel = CreateViewModel();

        Assert.False(viewModel.IsLeftSidebarCollapsed);
        Assert.True(viewModel.IsLeftSidebarExpanded);
        Assert.Equal(252, viewModel.LeftSidebarWidth);
        Assert.False(viewModel.IsRightSidebarCollapsed);
        Assert.True(viewModel.IsRightSidebarExpanded);
        Assert.Equal(326, viewModel.RightSidebarWidth);
    }

    [Fact]
    public void ToggleLeftSidebarCollapsesAndExpands()
    {
        var viewModel = CreateViewModel();

        viewModel.ToggleLeftSidebarCommand.Execute(null);

        Assert.True(viewModel.IsLeftSidebarCollapsed);
        Assert.False(viewModel.IsLeftSidebarExpanded);
        Assert.Equal(42, viewModel.LeftSidebarWidth);

        viewModel.ToggleLeftSidebarCommand.Execute(null);

        Assert.False(viewModel.IsLeftSidebarCollapsed);
        Assert.True(viewModel.IsLeftSidebarExpanded);
        Assert.Equal(252, viewModel.LeftSidebarWidth);
    }

    [Fact]
    public void ToggleRightSidebarCollapsesAndExpands()
    {
        var viewModel = CreateViewModel();

        viewModel.ToggleRightSidebarCommand.Execute(null);

        Assert.True(viewModel.IsRightSidebarCollapsed);
        Assert.False(viewModel.IsRightSidebarExpanded);
        Assert.Equal(42, viewModel.RightSidebarWidth);

        viewModel.ToggleRightSidebarCommand.Execute(null);

        Assert.False(viewModel.IsRightSidebarCollapsed);
        Assert.True(viewModel.IsRightSidebarExpanded);
        Assert.Equal(326, viewModel.RightSidebarWidth);
    }

    [Fact]
    public void CommandPaletteInitialListContainsExpectedCommands()
    {
        var viewModel = CreateViewModel();

        Assert.Contains(viewModel.CommandPaletteItems, item => item.Title == "Bold");
        Assert.Contains(viewModel.CommandPaletteItems, item => item.Title == "Show Split");
        Assert.Contains(viewModel.CommandPaletteItems, item => item.Title == "Insert Table");
        Assert.Contains(viewModel.CommandPaletteItems, item => item.Title == "Save");
    }

    [Fact]
    public void CommandPaletteFiltersCommandsByText()
    {
        var viewModel = CreateViewModel();

        viewModel.OpenCommandPaletteCommand.Execute(null);
        viewModel.CommandPaletteSearchText = "split";

        Assert.Contains(viewModel.FilteredCommandPaletteItems, item => item.Title == "Show Split");
        Assert.DoesNotContain(viewModel.FilteredCommandPaletteItems, item => item.Title == "Bold");
    }

    [Fact]
    public void CommandPaletteFilterCanReturnNoResults()
    {
        var viewModel = CreateViewModel();

        viewModel.OpenCommandPaletteCommand.Execute(null);
        viewModel.CommandPaletteSearchText = "zzzz";

        Assert.Empty(viewModel.FilteredCommandPaletteItems);
        Assert.False(viewModel.HasCommandPaletteResults);
        Assert.Null(viewModel.SelectedCommandPaletteItem);
    }

    [Fact]
    public void CommandPaletteOpenAndCloseUpdatesState()
    {
        var viewModel = CreateViewModel();

        viewModel.OpenCommandPaletteCommand.Execute(null);
        Assert.True(viewModel.IsCommandPaletteOpen);

        viewModel.CloseCommandPaletteCommand.Execute(null);
        Assert.False(viewModel.IsCommandPaletteOpen);
        Assert.Equal(string.Empty, viewModel.CommandPaletteSearchText);
    }

    [Fact]
    public void ExecutingCommandPaletteShowSplitChangesMode()
    {
        var viewModel = CreateViewModel();
        var item = viewModel.CommandPaletteItems.First(item => item.ActionKind == CommandPaletteActionKind.ShowSplit);

        Assert.True(viewModel.ExecuteCommandPaletteItem(item));

        Assert.Equal(DocumentViewMode.Split, viewModel.DocumentViewMode);
    }

    [Fact]
    public void ExecutingCommandPaletteShowPreviewChangesMode()
    {
        var viewModel = CreateViewModel();
        var item = viewModel.CommandPaletteItems.First(item => item.ActionKind == CommandPaletteActionKind.ShowPreview);

        Assert.True(viewModel.ExecuteCommandPaletteItem(item));

        Assert.Equal(DocumentViewMode.Preview, viewModel.DocumentViewMode);
    }

    [Fact]
    public void ExecutingCommandPaletteShowEditorChangesMode()
    {
        var viewModel = CreateViewModel();
        viewModel.ShowPreviewCommand.Execute(null);
        var item = viewModel.CommandPaletteItems.First(item => item.ActionKind == CommandPaletteActionKind.ShowEditor);

        Assert.True(viewModel.ExecuteCommandPaletteItem(item));

        Assert.Equal(DocumentViewMode.Editor, viewModel.DocumentViewMode);
    }

    [Fact]
    public void ExecutingCommandPaletteSaveSavesActiveNote()
    {
        var viewModel = CreateViewModel();
        var item = viewModel.CommandPaletteItems.First(item => item.ActionKind == CommandPaletteActionKind.Save);
        viewModel.Markdown = "# Alpha salva pela palette";

        Assert.True(viewModel.ExecuteCommandPaletteItem(item));

        Assert.Contains("# Alpha salva pela palette", File.ReadAllText(Path.Combine(_tempRepositoryPath, "A.md")));
        Assert.Equal("Salvo", viewModel.Status);
    }

    [Fact]
    public void SwitchingTabsPreservesDocumentViewMode()
    {
        var viewModel = CreateViewModel();
        viewModel.ShowSplitCommand.Execute(null);

        SelectNode(viewModel, "B.md");

        Assert.Equal("Beta", viewModel.ActiveTab?.Title);
        Assert.Equal(DocumentViewMode.Split, viewModel.DocumentViewMode);
        Assert.True(viewModel.HasEditorVisible);
        Assert.True(viewModel.HasPreviewVisible);
    }

    [Fact]
    public void EditingActiveTabKeepsPreviewBlocksUpdatedForSplitMode()
    {
        var viewModel = CreateViewModel();
        viewModel.ShowSplitCommand.Execute(null);

        viewModel.Markdown = """
        ### Teste

        Texto com **bold** e *italic*
        """;

        Assert.Contains(viewModel.PreviewBlocks, block => block is MarkdownHeadingBlock { Level: 3, Text: "Teste" });
        Assert.Contains(viewModel.PreviewBlocks, block => block is MarkdownParagraphBlock paragraph
            && paragraph.Inlines.Any(run => run is { Text: "bold", IsBold: true })
            && paragraph.Inlines.Any(run => run is { Text: "italic", IsItalic: true }));
    }

    [Fact]
    public void SelectingNoteOpensTabAndSelectingSameNoteDoesNotDuplicateIt()
    {
        var viewModel = CreateViewModel();

        SelectNode(viewModel, "B.md");
        SelectNode(viewModel, "A.md");
        SelectNode(viewModel, "B.md");

        Assert.Equal(2, viewModel.OpenTabs.Count);
        Assert.Equal("Beta", viewModel.ActiveTab?.Title);
    }

    [Fact]
    public void CloseOtherTabsCommandKeepsTargetTabOpen()
    {
        var viewModel = CreateViewModel();
        SelectNode(viewModel, "B.md");
        var alphaTab = viewModel.OpenTabs.First(tab => tab.Title == "Alpha");

        viewModel.CloseOtherTabsCommand.Execute(alphaTab);

        Assert.Single(viewModel.OpenTabs);
        Assert.Equal("Alpha", viewModel.ActiveTab?.Title);
        Assert.Same(alphaTab, viewModel.ActiveTab);
    }

    [Fact]
    public void CloseAllTabsCommandClosesEveryOpenTab()
    {
        var viewModel = CreateViewModel();
        SelectNode(viewModel, "B.md");

        viewModel.CloseAllTabsCommand.Execute(null);

        Assert.Empty(viewModel.OpenTabs);
        Assert.Null(viewModel.ActiveTab);
        Assert.Null(viewModel.SelectedNote);
        Assert.Equal("Todas as abas fechadas", viewModel.Status);
    }

    [Fact]
    public void ExplorerOpenCommandSelectsTargetNode()
    {
        var viewModel = CreateViewModel();
        var betaNode = FindNode(viewModel.Nodes, "B.md");
        Assert.NotNull(betaNode);

        viewModel.OpenExplorerItemCommand.Execute(betaNode);

        Assert.Same(betaNode, viewModel.SelectedNode);
        Assert.Equal("Beta", viewModel.ActiveTab?.Title);
    }

    [Fact]
    public void SwitchingTabsPreservesDirtyMarkdownWithoutSaving()
    {
        var viewModel = CreateViewModel();

        viewModel.Markdown = "# Alpha alterada";
        SelectNode(viewModel, "B.md");
        viewModel.OpenTabs.First(tab => tab.Title == "Alpha").ActivateCommand.Execute(null);

        Assert.Equal("# Alpha alterada", viewModel.Markdown);
        Assert.Contains(viewModel.OpenTabs, tab => tab.Title == "Alpha" && tab.IsDirty);
        Assert.DoesNotContain("# Alpha alterada", File.ReadAllText(Path.Combine(_tempRepositoryPath, "A.md")));
    }

    [Fact]
    public void SaveCommandWritesOnlyActiveTab()
    {
        var viewModel = CreateViewModel();

        viewModel.Markdown = "# Alpha alterada";
        SelectNode(viewModel, "B.md");
        viewModel.Markdown = "# Beta salva";
        viewModel.SaveNoteCommand.Execute(null);

        Assert.DoesNotContain("# Alpha alterada", File.ReadAllText(Path.Combine(_tempRepositoryPath, "A.md")));
        Assert.Contains("# Beta salva", File.ReadAllText(Path.Combine(_tempRepositoryPath, "B.md")));
        Assert.Contains(viewModel.OpenTabs, tab => tab.Title == "Alpha" && tab.IsDirty);
        Assert.Contains(viewModel.OpenTabs, tab => tab.Title == "Beta" && !tab.IsDirty);
    }

    [Fact]
    public void ClosingDirtyTabSavesItBeforeRemovingIt()
    {
        var viewModel = CreateViewModel();

        viewModel.Markdown = "# Alpha fechada salva";
        viewModel.ActiveTab!.CloseCommand.Execute(null);

        Assert.Empty(viewModel.OpenTabs);
        Assert.Contains("# Alpha fechada salva", File.ReadAllText(Path.Combine(_tempRepositoryPath, "A.md")));
    }

    [Fact]
    public void ClosingDirtyTabKeepsItOpenWhenSaveFails()
    {
        var repository = new FailingSaveRepository();
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Alteracao nao perdida"
        };

        viewModel.ActiveTab!.CloseCommand.Execute(null);

        Assert.Single(viewModel.OpenTabs);
        Assert.Equal("Erro ao salvar", viewModel.Status);
        Assert.Equal("# Alteracao nao perdida", viewModel.Markdown);
    }

    [Fact]
    public void ClosingLastTabClearsEditorState()
    {
        var viewModel = CreateViewModel();

        viewModel.ActiveTab!.CloseCommand.Execute(null);

        Assert.Empty(viewModel.OpenTabs);
        Assert.Null(viewModel.ActiveTab);
        Assert.Null(viewModel.SelectedNote);
        Assert.Equal(string.Empty, viewModel.Markdown);
    }

    [Fact]
    public async Task RenameOpenNoteUpdatesOpenTabPathAndSelection()
    {
        var viewModel = CreateViewModel(new TestTextPromptService("Gamma"));
        SelectNode(viewModel, "A.md");

        viewModel.RenameSelectedItemCommand.Execute(null);
        await WaitForStatusAsync(viewModel, "Item renomeado");

        Assert.Equal("Gamma.md", viewModel.ActiveTab?.Path);
        Assert.Equal("Gamma.md", viewModel.SelectedNode?.Path);
        Assert.Equal("Alpha", viewModel.ActiveTab?.Title);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Gamma.md")));
    }

    [Fact]
    public void DeleteOpenNoteClosesItsTabAndKeepsTrashOutOfTree()
    {
        var viewModel = CreateViewModel();
        SelectNode(viewModel, "A.md");

        viewModel.DeleteSelectedItemCommand.Execute(null);

        Assert.DoesNotContain(viewModel.OpenTabs, tab => tab.Path == "A.md");
        Assert.DoesNotContain(viewModel.Nodes, node => node.Path.StartsWith(".reponotes-trash", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(viewModel.TagFilters, tag => tag.Name == "one");
    }

    [Fact]
    public void DeletePermanentlyWithNoTrashSelectionDoesNotCrash()
    {
        var viewModel = CreateViewModel();

        viewModel.DeletePermanentlyCommand.Execute(null);

        Assert.Equal("Selecione um item da lixeira", viewModel.Status);
        Assert.Null(viewModel.SelectedTrashItem);
    }

    [Fact]
    public void DeletePermanentlyMissingTrashItemClearsSelection()
    {
        var viewModel = CreateViewModel();
        SelectNode(viewModel, "A.md");
        viewModel.DeleteSelectedItemCommand.Execute(null);
        var trashItem = viewModel.SelectedTrashItem;
        Assert.NotNull(trashItem);
        File.Delete(Path.Combine(_tempRepositoryPath, trashItem.TrashPath));

        viewModel.DeletePermanentlyCommand.Execute(null);

        Assert.Empty(viewModel.TrashItems);
        Assert.Null(viewModel.SelectedTrashItem);
        Assert.StartsWith("Item excluido permanentemente", viewModel.Status, StringComparison.Ordinal);
    }

    [Fact]
    public void EmptyTrashClearsTrashItemsAndSelection()
    {
        var viewModel = CreateViewModel();
        SelectNode(viewModel, "A.md");
        viewModel.DeleteSelectedItemCommand.Execute(null);
        SelectNode(viewModel, "B.md");
        viewModel.DeleteSelectedItemCommand.Execute(null);

        viewModel.EmptyTrashCommand.Execute(null);

        Assert.Empty(viewModel.TrashItems);
        Assert.Null(viewModel.SelectedTrashItem);
        Assert.Equal("Lixeira esvaziada", viewModel.Status);
    }

    [Fact]
    public void PreviewAndInternalLinksFollowActiveTab()
    {
        var viewModel = CreateViewModel();

        Assert.True(viewModel.HasInternalLinks);
        Assert.Contains(viewModel.InternalLinks, link => link.Target == "Beta" && link.IsResolved);
        SelectNode(viewModel, "B.md");

        Assert.Equal("Beta", viewModel.ActiveTab?.Title);
        Assert.False(viewModel.HasInternalLinks);
        Assert.Contains(viewModel.PreviewBlocks, block => block.GetType().Name.Contains("Heading", StringComparison.Ordinal));
    }

    private MainWindowViewModel CreateViewModel(ITextPromptService? promptService = null) =>
        new(new LocalMarkdownNoteRepository(_tempRepositoryPath), textPromptService: promptService);

    private static void SelectNode(MainWindowViewModel viewModel, string path)
    {
        var node = FindNode(viewModel.Nodes, path);
        Assert.NotNull(node);
        viewModel.SelectedNode = node;
    }

    private static RepositoryNodeViewModel? FindNode(IEnumerable<RepositoryNodeViewModel> nodes, string path)
    {
        foreach (var node in nodes)
        {
            if (node.Path == path)
            {
                return node;
            }

            var child = FindNode(node.Children, path);
            if (child is not null)
            {
                return child;
            }
        }

        return null;
    }

    private static async Task WaitForStatusAsync(MainWindowViewModel viewModel, string statusPrefix)
    {
        for (var i = 0; i < 20; i++)
        {
            if (viewModel.Status.StartsWith(statusPrefix, StringComparison.Ordinal))
            {
                return;
            }

            await Task.Delay(25);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }

    private sealed class TestTextPromptService(string value) : ITextPromptService
    {
        public Task<string?> PromptAsync(string title, string message, string initialValue) =>
            Task.FromResult<string?>(value);
    }

    private sealed class FailingSaveRepository : INoteRepository
    {
        private readonly NoteItem _note = new()
        {
            Id = "A.md",
            Title = "Alpha",
            Markdown = "# Alpha",
            Path = "A.md",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        public RepositoryItem CurrentRepository { get; } = new()
        {
            Id = "test",
            Name = "Test Repository",
            RootPath = "test"
        };

        public IReadOnlyList<RepositoryNode> GetTree() =>
        [
            new()
            {
                Name = "A.md",
                Type = RepositoryNodeType.Note,
                NoteId = "A.md",
                Path = "A.md"
            }
        ];

        public IReadOnlyList<NoteItem> GetNotes() => [_note];

        public NoteItem? GetNoteById(string noteId) => noteId == _note.Id ? _note : null;

        public void SaveNote(NoteItem note) => throw new IOException("Falha simulada.");

        public NoteItem CreateNote(string? folderPath, string noteName = "Nova nota", NoteTemplate? template = null) => _note;

        public string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta") => folderName;

        public string RenameItem(string itemPath, string newName) => itemPath;

        public string MoveItemToTrash(string itemPath) => itemPath;

        public IReadOnlyList<TrashItem> GetTrashItems() => [];

        public string RestoreFromTrash(string trashPath) => trashPath;

        public void DeletePermanently(string trashPath)
        {
        }

        public void EmptyTrash()
        {
        }
    }
}

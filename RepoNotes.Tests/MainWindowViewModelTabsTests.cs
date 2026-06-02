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
        Assert.True(viewModel.IsEditorMode);
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

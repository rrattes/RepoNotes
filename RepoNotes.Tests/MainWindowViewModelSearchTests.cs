using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelSearchTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public MainWindowViewModelSearchTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(_tempRepositoryPath, "Apps"));
        Directory.CreateDirectory(Path.Combine(_tempRepositoryPath, "Runbooks"));
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "Apps", "Portal.md"), """
        ---
        title: Portal
        type: application
        tags: [app, producao]
        status: active
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Portal

        Documentacao da aplicacao principal.
        """);
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "Runbooks", "Nginx.md"), """
        ---
        title: Proxy
        type: runbook
        tags: [infra, producao]
        status: active
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Proxy

        Reiniciar servico nginx em incidente.
        """);
    }

    [Fact]
    public void SearchTextFiltersByTitlePathAndContentCaseInsensitive()
    {
        var viewModel = CreateViewModel();

        viewModel.SearchText = "NGINX";

        Assert.Equal("Busca: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
        Assert.False(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
    }

    [Fact]
    public void EmptySearchTextRestoresFullTree()
    {
        var viewModel = CreateViewModel();
        viewModel.SearchText = "portal";

        viewModel.SearchText = string.Empty;

        Assert.Equal("Busca limpa", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void ClearSearchCommandClearsSearchAndRestoresTree()
    {
        var viewModel = CreateViewModel();
        viewModel.SearchText = "portal";

        viewModel.ClearSearchCommand.Execute(null);

        Assert.Equal("Busca limpa", viewModel.Status);
        Assert.False(viewModel.HasSearchText);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void SearchTextFiltersByFolderPath()
    {
        var viewModel = CreateViewModel();

        viewModel.SearchText = "runbooks";

        Assert.Equal("Busca: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, "Runbooks"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void SearchTextWithNoResultsShowsEmptyState()
    {
        var viewModel = CreateViewModel();

        viewModel.SearchText = "sem-resultado";

        Assert.Equal("Nenhum resultado encontrado", viewModel.SearchFeedback);
        Assert.Equal("Busca: 0 resultados", viewModel.Status);
        Assert.True(viewModel.HasNoSearchResults);
        Assert.Empty(viewModel.Nodes);
    }

    [Fact]
    public async Task SearchTextAppliesAfterDebounceDelay()
    {
        var viewModel = new MainWindowViewModel(
            new LocalMarkdownNoteRepository(_tempRepositoryPath),
            searchDebounceDelay: TimeSpan.FromMilliseconds(25));

        viewModel.SearchText = "nginx";

        Assert.Equal("Buscando...", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));

        await Task.Delay(80);

        Assert.Equal("Busca: 1 resultado", viewModel.Status);
        Assert.False(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void TagFiltersAreLoadedFromRepositoryNotesWithCounts()
    {
        var viewModel = CreateViewModel();

        Assert.Contains(viewModel.TagFilters, tag => tag.Name == "producao" && tag.Count == 2);
        Assert.Contains(viewModel.TagFilters, tag => tag.Name == "infra" && tag.Count == 1);
        Assert.Contains(viewModel.TagFilters, tag => tag.Name == "app" && tag.Count == 1);
    }

    [Fact]
    public void SelectingTagFiltersTreeByTag()
    {
        var viewModel = CreateViewModel();
        var infraTag = viewModel.TagFilters.First(tag => tag.Name == "infra");

        infraTag.SelectCommand.Execute(null);

        Assert.Equal("Tag infra: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
        Assert.False(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(viewModel.HasTagFilter);
    }

    [Fact]
    public void SearchTextCombinesWithSelectedTagFilter()
    {
        var viewModel = CreateViewModel();
        viewModel.TagFilters.First(tag => tag.Name == "producao").SelectCommand.Execute(null);

        viewModel.SearchText = "portal";

        Assert.Equal("Tag producao: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.False(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void ClearTagFilterRestoresTree()
    {
        var viewModel = CreateViewModel();
        viewModel.TagFilters.First(tag => tag.Name == "infra").SelectCommand.Execute(null);

        viewModel.ClearTagFilterCommand.Execute(null);

        Assert.Equal("Busca limpa", viewModel.Status);
        Assert.False(viewModel.HasTagFilter);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void TagsFromTrashAreNotListed()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        var note = repository.GetNotes().First(candidate => candidate.Tags.Contains("infra"));
        repository.MoveItemToTrash(note.Path);

        var viewModel = new MainWindowViewModel(repository);

        Assert.DoesNotContain(viewModel.TagFilters, tag => tag.Name == "infra");
        Assert.Contains(viewModel.TagFilters, tag => tag.Name == "producao" && tag.Count == 1);
    }

    [Fact]
    public void SearchTextDoesNotReturnTrashContent()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        repository.MoveItemToTrash(@"Runbooks\Nginx.md");
        var viewModel = new MainWindowViewModel(repository, searchDebounceDelay: TimeSpan.Zero);

        viewModel.SearchText = "nginx";

        Assert.Equal("Nenhum resultado encontrado", viewModel.SearchFeedback);
        Assert.True(viewModel.HasNoSearchResults);
        Assert.False(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    private MainWindowViewModel CreateViewModel() =>
        new(new LocalMarkdownNoteRepository(_tempRepositoryPath), searchDebounceDelay: TimeSpan.Zero);

    private static bool ContainsNode(IEnumerable<RepositoryNodeViewModel> nodes, string path)
    {
        foreach (var node in nodes)
        {
            if (node.Path.Equals(path, StringComparison.OrdinalIgnoreCase)
                || ContainsNode(node.Children, path))
            {
                return true;
            }
        }

        return false;
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }
}

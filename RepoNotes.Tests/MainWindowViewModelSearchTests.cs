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
        # Portal

        Documentacao da aplicacao principal.
        """);
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "Runbooks", "Nginx.md"), """
        # Proxy

        Reiniciar servico nginx em incidente.
        """);
    }

    [Fact]
    public void SearchTextFiltersByTitlePathAndContentCaseInsensitive()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.SearchText = "NGINX";

        Assert.Equal("Busca: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
        Assert.False(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
    }

    [Fact]
    public void EmptySearchTextRestoresFullTree()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath))
        {
            SearchText = "portal"
        };

        viewModel.SearchText = string.Empty;

        Assert.Equal("Busca limpa", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, @"Apps\Portal.md"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

    [Fact]
    public void SearchTextFiltersByFolderPath()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.SearchText = "runbooks";

        Assert.Equal("Busca: 1 resultado", viewModel.Status);
        Assert.True(ContainsNode(viewModel.Nodes, "Runbooks"));
        Assert.True(ContainsNode(viewModel.Nodes, @"Runbooks\Nginx.md"));
    }

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

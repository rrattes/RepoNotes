using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelCreateTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public MainWindowViewModelCreateTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRepositoryPath);
    }

    [Fact]
    public void NewNoteCommandCreatesFileAndOpensItInEditor()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.NewNoteCommand.Execute(null);

        Assert.Equal("Nova nota.md", viewModel.NotePath);
        Assert.Equal("Nova nota", viewModel.Title);
        Assert.Equal("Nota criada: Nova nota.md", viewModel.Status);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota.md")));
    }

    [Fact]
    public void NewFolderCommandCreatesFolderAndRefreshesTree()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.NewFolderCommand.Execute(null);

        Assert.Equal("Pasta criada: Nova pasta", viewModel.Status);
        Assert.True(Directory.Exists(Path.Combine(_tempRepositoryPath, "Nova pasta")));
        Assert.Contains(viewModel.Nodes, node => node.Path == "Nova pasta");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }
}

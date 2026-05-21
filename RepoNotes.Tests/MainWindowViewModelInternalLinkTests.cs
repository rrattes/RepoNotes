using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelInternalLinkTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public MainWindowViewModelInternalLinkTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRepositoryPath);

        File.WriteAllText(Path.Combine(_tempRepositoryPath, "A-Origem.md"), """
        ---
        title: Origem
        type: note
        tags: []
        status: draft
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Origem

        Consulte [[Destino]] e [[Nao existe]].
        """);

        File.WriteAllText(Path.Combine(_tempRepositoryPath, "Z-Destino.md"), """
        ---
        title: Destino
        type: note
        tags: []
        status: draft
        created: 2026-05-20T10:00:00.0000000Z
        updated: 2026-05-20T10:00:00.0000000Z
        ---
        # Destino
        """);
    }

    [Fact]
    public void LoadsInternalLinksForSelectedNote()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        Assert.True(viewModel.HasInternalLinks);
        Assert.Contains(viewModel.InternalLinks, link => link.Target == "Destino" && link.IsResolved);
        Assert.Contains(viewModel.InternalLinks, link => link.Target == "Nao existe" && !link.IsResolved);
    }

    [Fact]
    public void OpenInternalLinkCommandSelectsLinkedNote()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));
        var link = viewModel.InternalLinks.First(candidate => candidate.Target == "Destino");

        link.OpenCommand.Execute(null);

        Assert.Equal("Destino", viewModel.SelectedNote?.Title);
        Assert.Equal("Link interno aberto: Destino", viewModel.Status);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }
}

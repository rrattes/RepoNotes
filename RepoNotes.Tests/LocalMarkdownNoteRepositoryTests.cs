using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class LocalMarkdownNoteRepositoryTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public LocalMarkdownNoteRepositoryTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRepositoryPath);
    }

    [Fact]
    public void CreatesSampleRepositoryWhenEmpty()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);

        Assert.NotEmpty(repository.GetNotes());
        Assert.NotEmpty(repository.GetTree());
        Assert.True(Directory.EnumerateFiles(_tempRepositoryPath, "*.md", SearchOption.AllDirectories).Count() >= 3);
    }

    [Fact]
    public void SavesMarkdownBackToDisk()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        var note = repository.GetNotes().First();
        var updatedMarkdown = note.Markdown + Environment.NewLine + "Linha salva pelo teste.";

        note.Markdown = updatedMarkdown;
        repository.SaveNote(note);

        var savedPath = Path.Combine(_tempRepositoryPath, note.Path);
        Assert.Equal(updatedMarkdown, File.ReadAllText(savedPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }
}

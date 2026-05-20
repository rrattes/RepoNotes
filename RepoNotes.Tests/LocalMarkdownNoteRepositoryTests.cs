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

    [Fact]
    public void CreatesNewNoteWithUniqueSafeName()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);

        var firstNote = repository.CreateNote(null);
        var secondNote = repository.CreateNote(null);

        Assert.Equal("Nova nota.md", firstNote.Path);
        Assert.Equal("Nova nota 2.md", secondNote.Path);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota.md")));
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota 2.md")));
    }

    [Fact]
    public void CreatesNewNoteInsideSelectedFolder()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);

        var note = repository.CreateNote("Inbox");

        Assert.Equal(@"Inbox\Nova nota.md", note.Path);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Inbox", "Nova nota.md")));
    }

    [Fact]
    public void CreatesNewFolderWithUniqueSafeNameAndShowsItInTree()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);

        var firstFolderPath = repository.CreateFolder(null, "Ops:Runbooks");
        var secondFolderPath = repository.CreateFolder(null, "Ops:Runbooks");

        Assert.Equal("Ops-Runbooks", firstFolderPath);
        Assert.Equal("Ops-Runbooks 2", secondFolderPath);
        Assert.True(Directory.Exists(Path.Combine(_tempRepositoryPath, "Ops-Runbooks")));
        Assert.True(Directory.Exists(Path.Combine(_tempRepositoryPath, "Ops-Runbooks 2")));
        Assert.Contains(repository.GetTree(), node => node.Path == "Ops-Runbooks");
    }

    [Fact]
    public void RenamesMarkdownFileOnDisk()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        var note = repository.GetNotes().First();

        var newPath = repository.RenameItem(note.Path, "Nota:Renomeada");

        Assert.EndsWith("Nota-Renomeada.md", newPath);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, newPath)));
        Assert.DoesNotContain(repository.GetNotes(), candidate => candidate.Path == note.Path);
        Assert.Contains(repository.GetNotes(), candidate => candidate.Path == newPath);
    }

    [Fact]
    public void MovesDeletedNoteToRepositoryTrash()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        var note = repository.GetNotes().First();

        var trashPath = repository.MoveItemToTrash(note.Path);

        Assert.StartsWith(@".reponotes-trash\", trashPath);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, trashPath)));
        Assert.DoesNotContain(repository.GetNotes(), candidate => candidate.Path == note.Path);
        Assert.DoesNotContain(repository.GetTree(), node => node.Path == ".reponotes-trash");
    }

    [Fact]
    public void DoesNotAllowDeletingRepositoryRoot()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);

        Assert.Throws<InvalidOperationException>(() => repository.MoveItemToTrash(string.Empty));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }
}

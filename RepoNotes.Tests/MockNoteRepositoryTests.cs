using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MockNoteRepositoryTests
{
    [Fact]
    public void RepositoryStartsWithNotesAndTree()
    {
        var repository = new MockNoteRepository();

        Assert.NotEmpty(repository.GetNotes());
        Assert.NotEmpty(repository.GetTree());
        Assert.Equal("Meu Repositorio Local", repository.CurrentRepository.Name);
    }

    [Fact]
    public void CanFindNoteById()
    {
        var repository = new MockNoteRepository();

        var note = repository.GetNoteById("welcome");

        Assert.NotNull(note);
        Assert.Contains("RepoNotes", note.Title);
        Assert.True(note.WordCount > 0);
    }
}

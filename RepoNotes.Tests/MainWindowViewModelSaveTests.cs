using RepoNotes.App.ViewModels;
using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelSaveTests
{
    [Fact]
    public void SaveCommandDoesNotWriteWhenNoteIsUnchanged()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository);

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(0, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal(string.Empty, viewModel.LastErrorMessage);
    }

    [Fact]
    public void SaveCommandMarksChangedNoteAsSaved()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Nota atualizada"
        };

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal(string.Empty, viewModel.LastErrorMessage);
    }

    [Fact]
    public void SaveCommandKeepsEditedContentAndShowsErrorWhenSaveFails()
    {
        var repository = new TestNoteRepository
        {
            SaveException = new IOException("Arquivo bloqueado para teste.")
        };
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Conteudo que nao deve ser perdido"
        };

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Erro ao salvar", viewModel.Status);
        Assert.Equal("Arquivo bloqueado para teste.", viewModel.LastErrorMessage);
        Assert.Equal("# Conteudo que nao deve ser perdido", viewModel.Markdown);
    }

    private sealed class TestNoteRepository : INoteRepository
    {
        private readonly NoteItem _note = new()
        {
            Id = "note",
            Title = "Nota",
            Markdown = "# Nota",
            Path = "Nota.md",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        public RepositoryItem CurrentRepository { get; } = new()
        {
            Id = "test",
            Name = "Test Repository",
            RootPath = "test"
        };

        public int SaveCount { get; private set; }

        public Exception? SaveException { get; init; }

        public IReadOnlyList<RepositoryNode> GetTree() =>
        [
            new RepositoryNode
            {
                Name = "Nota.md",
                Type = RepositoryNodeType.Note,
                NoteId = "note",
                Path = "Nota.md"
            }
        ];

        public IReadOnlyList<NoteItem> GetNotes() => [_note];

        public NoteItem? GetNoteById(string noteId) => noteId == _note.Id ? _note : null;

        public void SaveNote(NoteItem note)
        {
            SaveCount++;
            if (SaveException is not null)
            {
                throw SaveException;
            }
        }
    }
}

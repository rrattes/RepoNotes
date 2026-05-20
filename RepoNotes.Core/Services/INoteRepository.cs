using RepoNotes.Core.Models;

namespace RepoNotes.Core.Services;

public interface INoteRepository
{
    RepositoryItem CurrentRepository { get; }

    IReadOnlyList<RepositoryNode> GetTree();

    IReadOnlyList<NoteItem> GetNotes();

    NoteItem? GetNoteById(string noteId);
}

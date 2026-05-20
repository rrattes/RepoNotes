using RepoNotes.Core.Models;

namespace RepoNotes.Core.Services;

public interface INoteRepository
{
    RepositoryItem CurrentRepository { get; }

    IReadOnlyList<RepositoryNode> GetTree();

    IReadOnlyList<NoteItem> GetNotes();

    NoteItem? GetNoteById(string noteId);

    void SaveNote(NoteItem note);

    NoteItem CreateNote(string? folderPath, string noteName = "Nova nota");

    string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta");

    string RenameItem(string itemPath, string newName);

    string MoveItemToTrash(string itemPath);
}

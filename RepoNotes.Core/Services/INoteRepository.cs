using RepoNotes.Core.Models;

namespace RepoNotes.Core.Services;

public interface INoteRepository
{
    RepositoryItem CurrentRepository { get; }

    IReadOnlyList<RepositoryNode> GetTree();

    IReadOnlyList<NoteItem> GetNotes();

    NoteItem? GetNoteById(string noteId);

    void SaveNote(NoteItem note);

    NoteItem CreateNote(string? folderPath, string noteName = "Nova nota", NoteTemplate? template = null);

    string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta");

    string RenameItem(string itemPath, string newName);

    string MoveItemToTrash(string itemPath);

    IReadOnlyList<TrashItem> GetTrashItems();

    string RestoreFromTrash(string trashPath);

    void DeletePermanently(string trashPath);

    void EmptyTrash();
}

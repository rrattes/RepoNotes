using System.Collections.ObjectModel;
using RepoNotes.Core.Models;

namespace RepoNotes.App.ViewModels;

public sealed class RepositoryNodeViewModel
{
    public RepositoryNodeViewModel(RepositoryNode node, ISet<string>? highlightedNoteIds = null)
    {
        Name = node.Name;
        Type = node.Type;
        NoteId = node.NoteId;
        Path = node.Path ?? node.Name;
        IsSearchMatch = node.NoteId is not null && highlightedNoteIds?.Contains(node.NoteId) == true;
        Children = new ObservableCollection<RepositoryNodeViewModel>(
            node.Children.Select(child => new RepositoryNodeViewModel(child, highlightedNoteIds)));
    }

    public string Name { get; }

    public RepositoryNodeType Type { get; }

    public string? NoteId { get; }

    public string Path { get; }

    public bool IsNote => Type == RepositoryNodeType.Note;

    public bool IsSearchMatch { get; }

    public ObservableCollection<RepositoryNodeViewModel> Children { get; }
}

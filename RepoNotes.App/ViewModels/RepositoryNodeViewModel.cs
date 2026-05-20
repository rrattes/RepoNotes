using System.Collections.ObjectModel;
using RepoNotes.Core.Models;

namespace RepoNotes.App.ViewModels;

public sealed class RepositoryNodeViewModel
{
    public RepositoryNodeViewModel(RepositoryNode node)
    {
        Name = node.Name;
        Type = node.Type;
        NoteId = node.NoteId;
        Path = node.Path ?? node.Name;
        Children = new ObservableCollection<RepositoryNodeViewModel>(
            node.Children.Select(child => new RepositoryNodeViewModel(child)));
    }

    public string Name { get; }

    public RepositoryNodeType Type { get; }

    public string? NoteId { get; }

    public string Path { get; }

    public bool IsNote => Type == RepositoryNodeType.Note;

    public ObservableCollection<RepositoryNodeViewModel> Children { get; }
}

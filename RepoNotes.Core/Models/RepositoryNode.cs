namespace RepoNotes.Core.Models;

public sealed class RepositoryNode
{
    public required string Name { get; init; }

    public required RepositoryNodeType Type { get; init; }

    public string? NoteId { get; init; }

    public string? Path { get; init; }

    public IReadOnlyList<RepositoryNode> Children { get; init; } = [];
}

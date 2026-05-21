namespace RepoNotes.Core.Models;

public sealed class TrashItem
{
    public required string Name { get; init; }

    public required string TrashPath { get; init; }

    public required string OriginalPath { get; init; }

    public bool IsNote { get; init; }
}

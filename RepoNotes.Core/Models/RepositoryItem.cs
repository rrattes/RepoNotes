namespace RepoNotes.Core.Models;

public sealed class RepositoryItem
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string RootPath { get; init; }
}

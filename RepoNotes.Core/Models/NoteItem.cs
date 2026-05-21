namespace RepoNotes.Core.Models;

public sealed class NoteItem
{
    public required string Id { get; init; }

    public required string Title { get; set; }

    public required string Markdown { get; set; }

    public required string Path { get; init; }

    public string Type { get; set; } = "note";

    public string Status { get; set; } = "draft";

    public required DateTime CreatedAt { get; init; }

    public required DateTime UpdatedAt { get; set; }

    public IReadOnlyList<string> Tags { get; set; } = [];

    public int WordCount =>
        Markdown.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
}

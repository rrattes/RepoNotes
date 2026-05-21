namespace RepoNotes.Core.Models;

public sealed class NoteTemplate
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string SuggestedType { get; init; }

    public IReadOnlyList<string> SuggestedTags { get; init; } = [];

    public required string MarkdownBody { get; init; }

    public string CreateMarkdown(string title) =>
        MarkdownBody.Replace("{{title}}", title, StringComparison.Ordinal);
}

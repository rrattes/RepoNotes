using System.Text.RegularExpressions;
using RepoNotes.Core.Models;

namespace RepoNotes.App.Services;

public sealed partial class InternalLinkService
{
    public IReadOnlyList<InternalLinkResult> ResolveLinks(string markdown, IEnumerable<NoteItem> notes)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return [];
        }

        var noteList = notes.ToArray();
        var links = new List<InternalLinkResult>();
        var seenTargets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (Match match in WikiLinkRegex().Matches(markdown))
        {
            var target = match.Groups["target"].Value.Trim();
            if (string.IsNullOrWhiteSpace(target) || !seenTargets.Add(target))
            {
                continue;
            }

            var resolvedNote = ResolveNote(target, noteList);
            links.Add(new InternalLinkResult
            {
                Target = target,
                DisplayText = target,
                NoteId = resolvedNote?.Id,
                NotePath = resolvedNote?.Path,
                IsResolved = resolvedNote is not null
            });
        }

        return links;
    }

    private static NoteItem? ResolveNote(string target, IEnumerable<NoteItem> notes)
    {
        var normalizedTarget = NormalizeTarget(target);

        return notes.FirstOrDefault(note => string.Equals(NormalizeTarget(note.Title), normalizedTarget, StringComparison.OrdinalIgnoreCase))
            ?? notes.FirstOrDefault(note => string.Equals(NormalizeTarget(Path.GetFileNameWithoutExtension(note.Path)), normalizedTarget, StringComparison.OrdinalIgnoreCase))
            ?? notes.FirstOrDefault(note => string.Equals(NormalizeTarget(Path.GetFileName(note.Path)), normalizedTarget, StringComparison.OrdinalIgnoreCase));
    }

    private static string NormalizeTarget(string value) =>
        Path.GetFileNameWithoutExtension(value.Trim()).Trim();

    [GeneratedRegex(@"\[\[(?<target>[^\]\r\n]+)\]\]")]
    private static partial Regex WikiLinkRegex();
}

public sealed class InternalLinkResult
{
    public required string Target { get; init; }

    public required string DisplayText { get; init; }

    public string? NoteId { get; init; }

    public string? NotePath { get; init; }

    public bool IsResolved { get; init; }
}

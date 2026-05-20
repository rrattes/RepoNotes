using System.Collections.ObjectModel;

namespace RepoNotes.App.ViewModels;

public abstract class MarkdownPreviewBlock;

public sealed class MarkdownHeadingBlock : MarkdownPreviewBlock
{
    public required string Text { get; init; }

    public int Level { get; init; }

    public double FontSize => Level switch
    {
        1 => 22,
        2 => 18,
        3 => 16,
        _ => 14
    };
}

public sealed class MarkdownParagraphBlock : MarkdownPreviewBlock
{
    public required string Text { get; init; }
}

public sealed class MarkdownListBlock : MarkdownPreviewBlock
{
    public ObservableCollection<MarkdownListItem> Items { get; } = [];
}

public sealed class MarkdownListItem
{
    public required string Text { get; init; }

    public bool IsTask { get; init; }

    public bool IsChecked { get; init; }

    public string Marker => IsTask ? (IsChecked ? "[x]" : "[ ]") : "-";
}

public sealed class MarkdownCodeBlock : MarkdownPreviewBlock
{
    public required string Text { get; init; }
}

public sealed class MarkdownQuoteBlock : MarkdownPreviewBlock
{
    public required string Text { get; init; }
}

public sealed class MarkdownTableBlock : MarkdownPreviewBlock
{
    public required string Text { get; init; }
}

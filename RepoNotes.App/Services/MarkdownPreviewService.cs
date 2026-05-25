using System.Text;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Extensions.Tables;
using Markdig.Extensions.TaskLists;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using RepoNotes.App.ViewModels;

namespace RepoNotes.App.Services;

public sealed partial class MarkdownPreviewService
{
    private readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public IReadOnlyList<MarkdownPreviewBlock> Render(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            return [new MarkdownParagraphBlock { Text = "Selecione uma nota para visualizar." }];
        }

        var document = Markdown.Parse(markdown, _pipeline);
        var blocks = new List<MarkdownPreviewBlock>();

        foreach (var block in document)
        {
            AddBlock(blocks, block);
        }

        return blocks.Count == 0
            ? [new MarkdownParagraphBlock { Text = markdown.Trim() }]
            : blocks;
    }

    private static void AddBlock(ICollection<MarkdownPreviewBlock> blocks, Block block)
    {
        switch (block)
        {
            case HeadingBlock heading:
                AddInlineBlock(blocks, ExtractInlineRuns(heading.Inline), inlines => new MarkdownHeadingBlock
                {
                    Text = GetInlineText(inlines),
                    Inlines = inlines,
                    Level = heading.Level
                });
                break;
            case ParagraphBlock paragraph:
                AddInlineBlock(blocks, ExtractInlineRuns(paragraph.Inline), inlines => new MarkdownParagraphBlock
                {
                    Text = GetInlineText(inlines),
                    Inlines = inlines
                });
                break;
            case ListBlock list:
                blocks.Add(CreateListBlock(list));
                break;
            case FencedCodeBlock fencedCode:
                AddTextBlock(blocks, ExtractLeafText(fencedCode), text => new MarkdownCodeBlock { Text = text });
                break;
            case CodeBlock code:
                AddTextBlock(blocks, ExtractLeafText(code), text => new MarkdownCodeBlock { Text = text });
                break;
            case QuoteBlock quote:
                AddInlineBlock(blocks, ExtractContainerInlines(quote), inlines => new MarkdownQuoteBlock
                {
                    Text = GetInlineText(inlines),
                    Inlines = inlines
                });
                break;
            case Table table:
                AddTextBlock(blocks, ExtractTableText(table), text => new MarkdownTableBlock { Text = text });
                break;
            case ThematicBreakBlock:
                blocks.Add(new MarkdownParagraphBlock { Text = "--------------------------------" });
                break;
            case ContainerBlock container:
                foreach (var child in container)
                {
                    AddBlock(blocks, child);
                }

                break;
        }
    }

    private static MarkdownListBlock CreateListBlock(ListBlock list)
    {
        var listBlock = new MarkdownListBlock();

        foreach (var item in list.OfType<ListItemBlock>())
        {
            var text = ExtractContainerText(item);
            var taskMatch = TaskItemRegex().Match(text);
            var taskList = FindTaskList(item);
            var inlines = ExtractContainerInlines(item);
            if (taskMatch.Success)
            {
                inlines = TrimTaskPrefix(inlines);
            }

            listBlock.Items.Add(new MarkdownListItem
            {
                Text = inlines.Count > 0
                    ? GetInlineText(inlines)
                    : taskMatch.Success
                        ? taskMatch.Groups["text"].Value
                        : text,
                Inlines = inlines,
                IsTask = taskMatch.Success || taskList is not null,
                IsChecked = taskList?.Checked
                    ?? (taskMatch.Success && taskMatch.Groups["checked"].Value.Equals("x", StringComparison.OrdinalIgnoreCase))
            });
        }

        return listBlock;
    }

    private static TaskList? FindTaskList(ContainerBlock item)
    {
        foreach (var block in item)
        {
            if (block is ParagraphBlock paragraph && paragraph.Inline is not null)
            {
                var taskList = FindTaskList(paragraph.Inline);
                if (taskList is not null)
                {
                    return taskList;
                }
            }
        }

        return null;
    }

    private static TaskList? FindTaskList(ContainerInline container)
    {
        foreach (var inline in container)
        {
            if (inline is TaskList taskList)
            {
                return taskList;
            }

            if (inline is ContainerInline nestedContainer)
            {
                var nestedTaskList = FindTaskList(nestedContainer);
                if (nestedTaskList is not null)
                {
                    return nestedTaskList;
                }
            }
        }

        return null;
    }

    private static string ExtractContainerText(ContainerBlock container)
    {
        var parts = new List<string>();

        foreach (var block in container)
        {
            var text = block switch
            {
                ParagraphBlock paragraph => ExtractInlineText(paragraph.Inline),
                HeadingBlock heading => ExtractInlineText(heading.Inline),
                LeafBlock leaf => ExtractLeafText(leaf),
                ContainerBlock nestedContainer => ExtractContainerText(nestedContainer),
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(text))
            {
                parts.Add(text);
            }
        }

        return string.Join(Environment.NewLine, parts);
    }

    private static IReadOnlyList<MarkdownInlineRun> ExtractContainerInlines(ContainerBlock container)
    {
        var runs = new List<MarkdownInlineRun>();

        foreach (var block in container)
        {
            var blockRuns = block switch
            {
                ParagraphBlock paragraph => ExtractInlineRuns(paragraph.Inline),
                HeadingBlock heading => ExtractInlineRuns(heading.Inline),
                LeafBlock leaf => [new MarkdownInlineRun { Text = ExtractLeafText(leaf) }],
                ContainerBlock nestedContainer => ExtractContainerInlines(nestedContainer),
                _ => []
            };

            if (blockRuns.Count == 0)
            {
                continue;
            }

            if (runs.Count > 0)
            {
                runs.Add(new MarkdownInlineRun { Text = Environment.NewLine });
            }

            runs.AddRange(blockRuns);
        }

        return NormalizeInlineRuns(runs);
    }

    private static string ExtractInlineText(ContainerInline? container)
    {
        if (container is null)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var inline in container)
        {
            AppendInlineText(builder, inline);
        }

        return builder.ToString().Trim();
    }

    private static IReadOnlyList<MarkdownInlineRun> ExtractInlineRuns(ContainerInline? container)
    {
        if (container is null)
        {
            return [];
        }

        var runs = new List<MarkdownInlineRun>();
        foreach (var inline in container)
        {
            AppendInlineRuns(runs, inline, isBold: false, isItalic: false, isLink: false, url: null);
        }

        return NormalizeInlineRuns(runs);
    }

    private static void AppendInlineRuns(
        ICollection<MarkdownInlineRun> runs,
        Inline inline,
        bool isBold,
        bool isItalic,
        bool isLink,
        string? url)
    {
        switch (inline)
        {
            case LiteralInline literal:
                runs.Add(new MarkdownInlineRun
                {
                    Text = literal.Content.ToString(),
                    IsBold = isBold,
                    IsItalic = isItalic,
                    IsLink = isLink,
                    Url = url
                });
                break;
            case CodeInline code:
                runs.Add(new MarkdownInlineRun
                {
                    Text = code.Content,
                    IsBold = isBold,
                    IsItalic = isItalic,
                    IsCode = true,
                    IsLink = isLink,
                    Url = url
                });
                break;
            case LineBreakInline:
                runs.Add(new MarkdownInlineRun { Text = Environment.NewLine });
                break;
            case LinkInline link:
                foreach (var child in link)
                {
                    AppendInlineRuns(runs, child, isBold, isItalic, isLink: true, link.Url);
                }

                break;
            case EmphasisInline emphasis:
                var emphasisIsBold = isBold || emphasis.DelimiterCount >= 2;
                var emphasisIsItalic = isItalic || emphasis.DelimiterCount is 1 or >= 3;
                foreach (var child in emphasis)
                {
                    AppendInlineRuns(runs, child, emphasisIsBold, emphasisIsItalic, isLink, url);
                }

                break;
            case ContainerInline container:
                foreach (var child in container)
                {
                    AppendInlineRuns(runs, child, isBold, isItalic, isLink, url);
                }

                break;
        }
    }

    private static IReadOnlyList<MarkdownInlineRun> NormalizeInlineRuns(IReadOnlyList<MarkdownInlineRun> runs)
    {
        var normalized = runs
            .Where(run => !string.IsNullOrEmpty(run.Text))
            .Select(run => new MarkdownInlineRun
            {
                Text = run.Text,
                IsBold = run.IsBold,
                IsItalic = run.IsItalic,
                IsCode = run.IsCode,
                IsLink = run.IsLink,
                Url = run.Url
            })
            .ToList();

        if (normalized.Count == 0)
        {
            return [];
        }

        normalized[0] = CopyWithText(normalized[0], normalized[0].Text.TrimStart());
        var lastIndex = normalized.Count - 1;
        normalized[lastIndex] = CopyWithText(normalized[lastIndex], normalized[lastIndex].Text.TrimEnd());

        return normalized.Where(run => run.Text.Length > 0).ToArray();
    }

    private static MarkdownInlineRun CopyWithText(MarkdownInlineRun run, string text) => new()
    {
        Text = text,
        IsBold = run.IsBold,
        IsItalic = run.IsItalic,
        IsCode = run.IsCode,
        IsLink = run.IsLink,
        Url = run.Url
    };

    private static string GetInlineText(IEnumerable<MarkdownInlineRun> runs)
    {
        return string.Concat(runs.Select(run => run.Text)).Trim();
    }

    private static IReadOnlyList<MarkdownInlineRun> TrimTaskPrefix(IReadOnlyList<MarkdownInlineRun> runs)
    {
        var text = GetInlineText(runs);
        var match = TaskItemRegex().Match(text);
        if (!match.Success)
        {
            return runs;
        }

        var charactersToSkip = text.Length - match.Groups["text"].Value.Length;
        var trimmed = new List<MarkdownInlineRun>();

        foreach (var run in runs)
        {
            if (charactersToSkip >= run.Text.Length)
            {
                charactersToSkip -= run.Text.Length;
                continue;
            }

            var runText = charactersToSkip > 0 ? run.Text[charactersToSkip..] : run.Text;
            charactersToSkip = 0;
            trimmed.Add(CopyWithText(run, runText));
        }

        return NormalizeInlineRuns(trimmed);
    }

    private static void AppendInlineText(StringBuilder builder, Inline inline)
    {
        switch (inline)
        {
            case LiteralInline literal:
                builder.Append(literal.Content.ToString());
                break;
            case CodeInline code:
                builder.Append('`').Append(code.Content).Append('`');
                break;
            case LineBreakInline:
                builder.AppendLine();
                break;
            case LinkInline link:
                var beforeLength = builder.Length;
                foreach (var child in link)
                {
                    AppendInlineText(builder, child);
                }

                if (!string.IsNullOrWhiteSpace(link.Url) && builder.Length > beforeLength)
                {
                    builder.Append(" (").Append(link.Url).Append(')');
                }

                break;
            case EmphasisInline emphasis:
                var marker = emphasis.DelimiterCount == 2 ? "**" : "*";
                builder.Append(marker);
                foreach (var child in emphasis)
                {
                    AppendInlineText(builder, child);
                }

                builder.Append(marker);
                break;
            case ContainerInline container:
                foreach (var child in container)
                {
                    AppendInlineText(builder, child);
                }

                break;
        }
    }

    private static string ExtractLeafText(LeafBlock block)
    {
        var lines = block.Lines.Lines;
        var builder = new StringBuilder();

        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index].Slice.ToString();
            builder.AppendLine(line);
        }

        return builder.ToString().TrimEnd();
    }

    private static string ExtractTableText(Table table)
    {
        var rows = new List<IReadOnlyList<string>>();

        foreach (var row in table.OfType<TableRow>())
        {
            rows.Add(row
                .OfType<TableCell>()
                .Select(cell => ExtractContainerText(cell).ReplaceLineEndings(" "))
                .ToArray());
        }

        if (rows.Count == 0)
        {
            return string.Empty;
        }

        var columnCount = rows.Max(row => row.Count);
        var widths = Enumerable.Range(0, columnCount)
            .Select(column => rows.Max(row => column < row.Count ? row[column].Length : 0))
            .ToArray();

        var builder = new StringBuilder();
        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];
            builder.Append("| ");
            for (var column = 0; column < columnCount; column++)
            {
                var cell = column < row.Count ? row[column] : string.Empty;
                builder.Append(cell.PadRight(widths[column])).Append(" | ");
            }

            builder.AppendLine();

            if (rowIndex == 0)
            {
                builder.Append("| ");
                foreach (var width in widths)
                {
                    builder.Append(new string('-', Math.Max(width, 3))).Append(" | ");
                }

                builder.AppendLine();
            }
        }

        return builder.ToString().TrimEnd();
    }

    private static void AddTextBlock(ICollection<MarkdownPreviewBlock> blocks, string text, Func<string, MarkdownPreviewBlock> createBlock)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            blocks.Add(createBlock(text));
        }
    }

    private static void AddInlineBlock(
        ICollection<MarkdownPreviewBlock> blocks,
        IReadOnlyList<MarkdownInlineRun> inlines,
        Func<IReadOnlyList<MarkdownInlineRun>, MarkdownPreviewBlock> createBlock)
    {
        if (inlines.Count > 0 && !string.IsNullOrWhiteSpace(GetInlineText(inlines)))
        {
            blocks.Add(createBlock(inlines));
        }
    }

    [GeneratedRegex(@"^\[(?<checked>[ xX])\]\s+(?<text>.*)$")]
    private static partial Regex TaskItemRegex();
}

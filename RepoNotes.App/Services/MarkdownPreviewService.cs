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
                AddTextBlock(blocks, ExtractInlineText(heading.Inline), text => new MarkdownHeadingBlock
                {
                    Text = text,
                    Level = heading.Level
                });
                break;
            case ParagraphBlock paragraph:
                AddTextBlock(blocks, ExtractInlineText(paragraph.Inline), text => new MarkdownParagraphBlock { Text = text });
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
                AddTextBlock(blocks, ExtractContainerText(quote), text => new MarkdownQuoteBlock { Text = text });
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
            listBlock.Items.Add(new MarkdownListItem
            {
                Text = taskMatch.Success ? taskMatch.Groups["text"].Value : text,
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

    [GeneratedRegex(@"^\[(?<checked>[ xX])\]\s+(?<text>.*)$")]
    private static partial Regex TaskItemRegex();
}

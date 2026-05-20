using RepoNotes.App.Services;
using RepoNotes.App.ViewModels;

namespace RepoNotes.Tests;

public sealed class MarkdownPreviewServiceTests
{
    [Fact]
    public void RendersCommonMarkdownBlocks()
    {
        var service = new MarkdownPreviewService();

        var blocks = service.Render("""
        # Titulo

        Paragrafo com [link](https://example.com).

        - item
        - [x] feito

        > aviso

        ```csharp
        Console.WriteLine("ok");
        ```
        """);

        Assert.Contains(blocks, block => block is MarkdownHeadingBlock { Text: "Titulo", Level: 1 });
        Assert.Contains(blocks, block => block is MarkdownParagraphBlock paragraph
            && paragraph.Text.Contains("https://example.com", StringComparison.Ordinal));
        Assert.Contains(blocks, block => block is MarkdownListBlock list
            && list.Items.Any(item => item.IsTask && item.IsChecked && item.Text == "feito"));
        Assert.Contains(blocks, block => block is MarkdownQuoteBlock { Text: "aviso" });
        Assert.Contains(blocks, block => block is MarkdownCodeBlock code
            && code.Text.Contains("Console.WriteLine", StringComparison.Ordinal));
    }

    [Fact]
    public void RendersPipeTablesAsMonospaceTableText()
    {
        var service = new MarkdownPreviewService();

        var blocks = service.Render("""
        | Nome | Status |
        | ---- | ------ |
        | API  | OK     |
        """);

        var table = Assert.IsType<MarkdownTableBlock>(Assert.Single(blocks));
        Assert.Contains("Nome", table.Text, StringComparison.Ordinal);
        Assert.Contains("API", table.Text, StringComparison.Ordinal);
    }
}

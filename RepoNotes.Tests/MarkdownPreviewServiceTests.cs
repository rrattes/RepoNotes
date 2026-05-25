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
            && paragraph.Inlines.Any(run => run is { IsLink: true, Url: "https://example.com", Text: "link" }));
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

    [Fact]
    public void IdentifiesBoldInline()
    {
        var paragraph = RenderSingleParagraph("Texto **bold** aqui.");

        Assert.Contains(paragraph.Inlines, run => run is { Text: "bold", IsBold: true, IsItalic: false });
        Assert.DoesNotContain("**", paragraph.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void IdentifiesItalicInline()
    {
        var paragraph = RenderSingleParagraph("Texto *italic* aqui.");

        Assert.Contains(paragraph.Inlines, run => run is { Text: "italic", IsBold: false, IsItalic: true });
        Assert.DoesNotContain("*italic*", paragraph.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void IdentifiesBoldItalicInline()
    {
        var paragraph = RenderSingleParagraph("Texto ***bold italic*** aqui.");

        Assert.Contains(paragraph.Inlines, run => run is { Text: "bold italic", IsBold: true, IsItalic: true });
    }

    [Fact]
    public void IdentifiesInlineCode()
    {
        var paragraph = RenderSingleParagraph("Use `dotnet build` agora.");

        Assert.Contains(paragraph.Inlines, run => run is { Text: "dotnet build", IsCode: true });
        Assert.DoesNotContain("`", paragraph.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void IdentifiesInlineLinkTextAndUrl()
    {
        var paragraph = RenderSingleParagraph("Abra [site](https://example.com).");

        Assert.Contains(paragraph.Inlines, run => run is
        {
            Text: "site",
            IsLink: true,
            Url: "https://example.com"
        });
    }

    [Fact]
    public void PreservesMixedInlineTextOrder()
    {
        var paragraph = RenderSingleParagraph("Texto **bold** e *italic* com `code`.");

        Assert.Equal("Texto bold e italic com code.", paragraph.Text);
        Assert.Contains(paragraph.Inlines, run => run is { Text: "bold", IsBold: true });
        Assert.Contains(paragraph.Inlines, run => run is { Text: "italic", IsItalic: true });
        Assert.Contains(paragraph.Inlines, run => run is { Text: "code", IsCode: true });
    }

    [Fact]
    public void HeadingsPreserveTextWithInlineRuns()
    {
        var service = new MarkdownPreviewService();

        var heading = Assert.IsType<MarkdownHeadingBlock>(Assert.Single(service.Render("# Titulo **forte**")));

        Assert.Equal("Titulo forte", heading.Text);
        Assert.Contains(heading.Inlines, run => run is { Text: "forte", IsBold: true });
    }

    [Fact]
    public void ListsPreserveTextWithInlineRuns()
    {
        var service = new MarkdownPreviewService();

        var list = Assert.IsType<MarkdownListBlock>(Assert.Single(service.Render("- item **forte**")));
        var item = Assert.Single(list.Items);

        Assert.Equal("item forte", item.Text);
        Assert.Contains(item.Inlines, run => run is { Text: "forte", IsBold: true });
    }

    [Fact]
    public void CodeBlocksAreNotConvertedToInlineCode()
    {
        var service = new MarkdownPreviewService();

        var codeBlock = Assert.IsType<MarkdownCodeBlock>(Assert.Single(service.Render("""
        ```
        var text = `not inline`;
        ```
        """)));

        Assert.Contains("`not inline`", codeBlock.Text, StringComparison.Ordinal);
    }

    [Fact]
    public void BlockquotesPreserveInlineRuns()
    {
        var service = new MarkdownPreviewService();

        var quote = Assert.IsType<MarkdownQuoteBlock>(Assert.Single(service.Render("> aviso **forte**")));

        Assert.Equal("aviso forte", quote.Text);
        Assert.Contains(quote.Inlines, run => run is { Text: "forte", IsBold: true });
    }

    private static MarkdownParagraphBlock RenderSingleParagraph(string markdown)
    {
        var service = new MarkdownPreviewService();

        return Assert.IsType<MarkdownParagraphBlock>(Assert.Single(service.Render(markdown)));
    }
}

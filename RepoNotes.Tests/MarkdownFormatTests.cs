using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MarkdownFormatTests
{
    private static MainWindowViewModel CreateViewModel() =>
        new(new MockNoteRepository());

    // ── Bold ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Bold_WithSelection_WrapsInDoubleAsterisks()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("hello world", 6, 11, "bold");
        Assert.Equal("hello **world**", text);
        Assert.Equal(8, s);
        Assert.Equal(13, e);
    }

    [Fact]
    public void Bold_WithoutSelection_InsertsMarkersWithCursorInside()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("hello", 5, 5, "bold");
        Assert.Equal("hello****", text);
        Assert.Equal(7, s);
        Assert.Equal(7, e);
    }

    // ── Italic ────────────────────────────────────────────────────────────────

    [Fact]
    public void Italic_WithSelection_WrapsInAsterisk()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("hi world", 3, 8, "italic");
        Assert.Equal("hi *world*", text);
        Assert.Equal(4, s);
        Assert.Equal(9, e);
    }

    [Fact]
    public void Italic_WithoutSelection_InsertsMarkersWithCursorInside()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("abc", 1, 1, "italic");
        Assert.Equal("a**bc", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    // ── Headings ──────────────────────────────────────────────────────────────

    [Fact]
    public void H1_OnPlainLine_InsertsPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("Hello world", 0, 0, "h1");
        Assert.Equal("# Hello world", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    [Fact]
    public void H1_OnH1Line_RemovesPrefixToggle()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("# Hello world", 2, 2, "h1");
        Assert.Equal("Hello world", text);
        Assert.Equal(0, s);
        Assert.Equal(0, e);
    }

    [Fact]
    public void H2_OnH1Line_ReplacesPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("# Hello world", 2, 2, "h2");
        Assert.Equal("## Hello world", text);
        Assert.Equal(3, s);
        Assert.Equal(3, e);
    }

    [Fact]
    public void H3_OnPlainLine_InsertsPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("content", 3, 3, "h3");
        Assert.Equal("### content", text);
        Assert.Equal(7, s);
        Assert.Equal(7, e);
    }

    [Fact]
    public void H3_OnH3Line_RemovesPrefixToggle()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("### heading", 4, 4, "h3");
        Assert.Equal("heading", text);
        Assert.Equal(0, s);
        Assert.Equal(0, e);
    }

    // ── List ──────────────────────────────────────────────────────────────────

    [Fact]
    public void List_OnPlainLine_InsertsListPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("item", 2, 2, "list");
        Assert.Equal("- item", text);
        Assert.Equal(4, s);
        Assert.Equal(4, e);
    }

    [Fact]
    public void List_OnListLine_RemovesListPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("- item", 4, 4, "list");
        Assert.Equal("item", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    // ── Checklist ─────────────────────────────────────────────────────────────

    [Fact]
    public void Checklist_OnPlainLine_InsertsChecklistPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("todo", 2, 2, "checklist");
        Assert.Equal("- [ ] todo", text);
        Assert.Equal(8, s);
        Assert.Equal(8, e);
    }

    [Fact]
    public void Checklist_OnChecklistLine_RemovesChecklistPrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("- [ ] todo", 8, 8, "checklist");
        Assert.Equal("todo", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    // ── Quote ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Quote_OnPlainLine_InsertsQuotePrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("text", 2, 2, "quote");
        Assert.Equal("> text", text);
        Assert.Equal(4, s);
        Assert.Equal(4, e);
    }

    [Fact]
    public void Quote_OnQuoteLine_RemovesQuotePrefix()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("> text", 4, 4, "quote");
        Assert.Equal("text", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    // ── Code ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Code_WithSelection_SingleLine_WrapsInBackticks()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("run command here", 4, 11, "code");
        Assert.Equal("run `command` here", text);
        Assert.Equal(5, s);
        Assert.Equal(12, e);
    }

    [Fact]
    public void Code_WithoutSelection_InsertsBackticksWithCursorInside()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("abc", 1, 1, "code");
        Assert.Equal("a``bc", text);
        Assert.Equal(2, s);
        Assert.Equal(2, e);
    }

    [Fact]
    public void Code_WithMultiLineSelection_WrapsInFenceBlock()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("before\nline1\nline2\nafter", 7, 18, "code");
        Assert.Equal("before\n```\nline1\nline2\n```\nafter", text);
        Assert.Equal(11, s);
        Assert.Equal(22, e);
    }

    // ── Link ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Link_WithSelection_WrapsAsMarkdownLink()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("see this", 4, 8, "link");
        Assert.Equal("see [this](url)", text);
        Assert.Equal(11, s);
        Assert.Equal(14, e);
    }

    [Fact]
    public void Link_WithoutSelection_InsertsDefaultLinkWithUrlSelected()
    {
        var vm = CreateViewModel();
        var (text, s, e) = vm.ApplyMarkdownFormat("go ", 3, 3, "link");
        Assert.Equal("go [texto](url)", text);
        Assert.Equal(11, s);
        Assert.Equal(14, e);
    }
}

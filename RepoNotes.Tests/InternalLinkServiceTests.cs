using RepoNotes.App.Services;
using RepoNotes.Core.Models;

namespace RepoNotes.Tests;

public sealed class InternalLinkServiceTests
{
    [Fact]
    public void DetectsWikiStyleInternalLinks()
    {
        var service = new InternalLinkService();

        var links = service.ResolveLinks("Veja [[Runbook de Deploy]] e [[Servidor Web]].", []);

        Assert.Collection(
            links,
            link => Assert.Equal("Runbook de Deploy", link.Target),
            link => Assert.Equal("Servidor Web", link.Target));
    }

    [Fact]
    public void ResolvesInternalLinkByTitle()
    {
        var service = new InternalLinkService();
        var notes = new[]
        {
            CreateNote("docs/runbook.md", "Runbook de Deploy")
        };

        var link = Assert.Single(service.ResolveLinks("[[Runbook de Deploy]]", notes));

        Assert.True(link.IsResolved);
        Assert.Equal("docs/runbook.md", link.NoteId);
    }

    [Fact]
    public void ResolvesInternalLinkByFileName()
    {
        var service = new InternalLinkService();
        var notes = new[]
        {
            CreateNote("infra/Servidor Web.md", "Servidor principal")
        };

        var link = Assert.Single(service.ResolveLinks("[[Servidor Web]]", notes));

        Assert.True(link.IsResolved);
        Assert.Equal("infra/Servidor Web.md", link.NotePath);
    }

    [Fact]
    public void IdentifiesBrokenInternalLink()
    {
        var service = new InternalLinkService();

        var link = Assert.Single(service.ResolveLinks("[[Nota inexistente]]", []));

        Assert.False(link.IsResolved);
        Assert.Null(link.NoteId);
    }

    private static NoteItem CreateNote(string path, string title) => new()
    {
        Id = path,
        Path = path,
        Title = title,
        Markdown = $"# {title}",
        Type = "note",
        Status = "draft",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now
    };
}

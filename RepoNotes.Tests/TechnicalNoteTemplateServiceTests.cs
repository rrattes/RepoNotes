using RepoNotes.Core.Services;

namespace RepoNotes.Tests;

public sealed class TechnicalNoteTemplateServiceTests
{
    [Fact]
    public void ListsInitialTechnicalTemplates()
    {
        var service = new TechnicalNoteTemplateService();

        var templates = service.GetTemplates();

        Assert.Equal(10, templates.Count);
        Assert.Contains(templates, template => template.Id == "free-note" && template.Name == "Nota livre");
        Assert.Contains(templates, template => template.Id == "runbook" && template.Name == "Runbook");
        Assert.Contains(templates, template => template.Id == "technical-handover" && template.Name == "Handover tecnico");
        Assert.Contains(templates, template => template.Id == "incident" && template.Name == "Incidente");
        Assert.Contains(templates, template => template.Id == "script" && template.Name == "Script");
        Assert.Contains(templates, template => template.Id == "prompt" && template.Name == "Prompt");
        Assert.Contains(templates, template => template.Id == "meeting" && template.Name == "Reuniao");
        Assert.Contains(templates, template => template.Id == "checklist" && template.Name == "Checklist");
        Assert.Contains(templates, template => template.Id == "application" && template.Name == "Aplicacao");
        Assert.Contains(templates, template => template.Id == "server" && template.Name == "Servidor");
    }

    [Fact]
    public void CreatesFreeNoteMarkdownBody()
    {
        var template = new TechnicalNoteTemplateService().GetDefaultTemplate();

        var markdown = template.CreateMarkdown("Minha nota");

        Assert.Equal("free-note", template.Id);
        Assert.Equal("note", template.SuggestedType);
        Assert.Contains("# Minha nota", markdown);
        Assert.Contains("Escreva sua nota aqui.", markdown);
    }

    [Fact]
    public void CreatesRunbookMarkdownWithExpectedSections()
    {
        var template = new TechnicalNoteTemplateService().GetTemplateById("runbook")!;

        var markdown = template.CreateMarkdown("Deploy local");

        Assert.Equal("runbook", template.SuggestedType);
        Assert.Contains("# Deploy local", markdown);
        Assert.Contains("## Objetivo", markdown);
        Assert.Contains("## Pre-requisitos", markdown);
        Assert.Contains("## Procedimento", markdown);
        Assert.Contains("## Validacao", markdown);
        Assert.Contains("## Rollback", markdown);
    }
}

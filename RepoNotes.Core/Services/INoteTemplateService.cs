using RepoNotes.Core.Models;

namespace RepoNotes.Core.Services;

public interface INoteTemplateService
{
    IReadOnlyList<NoteTemplate> GetTemplates();

    NoteTemplate GetDefaultTemplate();

    NoteTemplate? GetTemplateById(string templateId);
}

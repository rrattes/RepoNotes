namespace RepoNotes.App.Services;

public interface ITextPromptService
{
    Task<string?> PromptAsync(string title, string message, string initialValue);
}

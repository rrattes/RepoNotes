namespace RepoNotes.App.Services;

public sealed class NullTextPromptService : ITextPromptService
{
    public Task<string?> PromptAsync(string title, string message, string initialValue) =>
        Task.FromResult<string?>(initialValue);
}

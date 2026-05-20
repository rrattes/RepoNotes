namespace RepoNotes.App.Services;

public sealed class NullFolderPickerService : IFolderPickerService
{
    public Task<string?> PickRepositoryPathAsync() =>
        Task.FromResult<string?>(null);
}

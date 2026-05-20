namespace RepoNotes.App.Services;

public interface IFolderPickerService
{
    Task<string?> PickRepositoryPathAsync();
}

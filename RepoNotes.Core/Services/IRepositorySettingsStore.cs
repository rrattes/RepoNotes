namespace RepoNotes.Core.Services;

public interface IRepositorySettingsStore
{
    string? GetLastRepositoryPath();

    void SaveLastRepositoryPath(string repositoryPath);
}

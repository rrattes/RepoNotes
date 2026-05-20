using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class JsonRepositorySettingsStoreTests : IDisposable
{
    private readonly string _settingsPath = Path.Combine(
        Path.GetTempPath(),
        "RepoNotes.Tests",
        Guid.NewGuid().ToString("N"),
        "settings.json");

    [Fact]
    public void SavesAndLoadsLastRepositoryPath()
    {
        var repositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", "repository");
        var settingsStore = new JsonRepositorySettingsStore(_settingsPath);

        settingsStore.SaveLastRepositoryPath(repositoryPath);

        var reloadedSettingsStore = new JsonRepositorySettingsStore(_settingsPath);
        Assert.Equal(repositoryPath, reloadedSettingsStore.GetLastRepositoryPath());
    }

    [Fact]
    public void ReturnsNullWhenSettingsFileIsMissing()
    {
        var settingsStore = new JsonRepositorySettingsStore(_settingsPath);

        Assert.Null(settingsStore.GetLastRepositoryPath());
    }

    public void Dispose()
    {
        var directory = Path.GetDirectoryName(_settingsPath);
        if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}

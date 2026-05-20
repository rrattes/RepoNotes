using System.Text.Json;
using RepoNotes.Core.Services;

namespace RepoNotes.Storage;

public sealed class JsonRepositorySettingsStore : IRepositorySettingsStore
{
    private readonly string _settingsPath;

    public JsonRepositorySettingsStore(string? settingsPath = null)
    {
        _settingsPath = settingsPath ?? GetDefaultSettingsPath();
    }

    public string? GetLastRepositoryPath()
    {
        if (!File.Exists(_settingsPath))
        {
            return null;
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<RepositorySettings>(json);
            return string.IsNullOrWhiteSpace(settings?.LastRepositoryPath)
                ? null
                : settings.LastRepositoryPath;
        }
        catch (JsonException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }

    public void SaveLastRepositoryPath(string repositoryPath)
    {
        var directory = Path.GetDirectoryName(_settingsPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var settings = new RepositorySettings(repositoryPath);
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
    }

    private static string GetDefaultSettingsPath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "RepoNotes", "settings.json");
    }

    private sealed record RepositorySettings(string? LastRepositoryPath);
}

using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace RepoNotes.App.Services;

public sealed class AvaloniaFolderPickerService(Window owner) : IFolderPickerService
{
    public async Task<string?> PickRepositoryPathAsync()
    {
        var folders = await owner.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Abrir repositorio local",
            AllowMultiple = false
        });

        return folders.Count == 0
            ? null
            : folders[0].TryGetLocalPath();
    }
}

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RepoNotes.App.Services;
using RepoNotes.App.ViewModels;
using RepoNotes.App.Views;
using RepoNotes.Storage;

namespace RepoNotes.App;

public sealed partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var settingsStore = new JsonRepositorySettingsStore();
            var lastRepositoryPath = settingsStore.GetLastRepositoryPath();
            var initialStatus = string.Empty;

            if (!string.IsNullOrWhiteSpace(lastRepositoryPath) && !Directory.Exists(lastRepositoryPath))
            {
                initialStatus = "Repositorio anterior nao encontrado. Usando sample-repository.";
                lastRepositoryPath = null;
            }

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(
                new LocalMarkdownNoteRepository(lastRepositoryPath),
                new AvaloniaFolderPickerService(mainWindow),
                settingsStore,
                path => new LocalMarkdownNoteRepository(path),
                initialStatus);

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}

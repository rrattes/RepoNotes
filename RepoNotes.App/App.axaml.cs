using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(new MockNoteRepository())
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

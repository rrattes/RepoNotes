using Avalonia.Controls;
using Avalonia.Input;
using RepoNotes.App.ViewModels;

namespace RepoNotes.App.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        KeyDown += OnWindowKeyDown;
    }

    private void OnWindowKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.K && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            SearchBox.Focus();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape && SearchBox.IsFocused && DataContext is MainWindowViewModel viewModel)
        {
            viewModel.ClearSearchCommand.Execute(null);
            e.Handled = true;
        }
    }
}

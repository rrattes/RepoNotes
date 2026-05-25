using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using RepoNotes.App.ViewModels;

namespace RepoNotes.App.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        KeyDown += OnWindowKeyDown;
        PropertyChanged += (_, e) =>
        {
            if (e.Property == WindowStateProperty)
            {
                UpdateWindowStateChrome();
            }
        };

        Dispatcher.UIThread.Post(UpdateWindowStateChrome);
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

    private void ApplyToolbarFormat(string type)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var editor = MarkdownEditor;
        var text = editor.Text ?? string.Empty;
        var selStart = editor.SelectionStart;
        var selEnd = editor.SelectionEnd;

        var (newText, newSelStart, newSelEnd) = viewModel.ApplyMarkdownFormat(text, selStart, selEnd, type);

        viewModel.Markdown = newText;
        editor.SelectionStart = newSelStart;
        editor.SelectionEnd = newSelEnd;
        editor.Focus();
    }

    private void OnBoldClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("bold");
    private void OnItalicClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("italic");
    private void OnH1Click(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("h1");
    private void OnH2Click(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("h2");
    private void OnH3Click(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("h3");
    private void OnListClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("list");
    private void OnChecklistClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("checklist");
    private void OnLinkClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("link");
    private void OnCodeClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("code");
    private void OnQuoteClick(object? sender, RoutedEventArgs e) => ApplyToolbarFormat("quote");

    private void OnWindowBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximizeRestore();
                e.Handled = true;
                return;
            }

            BeginMoveDrag(e);
        }
    }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void OnMaximizeRestoreClick(object? sender, RoutedEventArgs e) => ToggleMaximizeRestore();

    private void OnCloseClick(object? sender, RoutedEventArgs e) => Close();

    private void ToggleMaximizeRestore()
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

        UpdateWindowStateChrome();
    }

    private void UpdateWindowStateChrome()
    {
        if (MaximizeRestoreButton is null)
        {
            return;
        }

        MaximizeRestoreButton.Content = WindowState == WindowState.Maximized ? "\u2750" : "\u25A1";
    }
}

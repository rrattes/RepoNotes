using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
}

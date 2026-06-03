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
        if (e.Key == Key.P
            && e.KeyModifiers.HasFlag(KeyModifiers.Control)
            && e.KeyModifiers.HasFlag(KeyModifiers.Shift)
            && DataContext is MainWindowViewModel paletteViewModel)
        {
            paletteViewModel.OpenCommandPaletteCommand.Execute(null);
            Dispatcher.UIThread.Post(() =>
            {
                CommandPaletteSearchBox.Focus();
                CommandPaletteSearchBox.SelectAll();
            });
            e.Handled = true;
            return;
        }

        if (e.Key == Key.K && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            if (TryGetFocusedMarkdownEditor(out var linkEditor))
            {
                ApplyToolbarFormat("link", linkEditor);
                e.Handled = true;
                return;
            }

            SearchBox.Focus();
            e.Handled = true;
            return;
        }

        if (TryHandleMarkdownShortcut(e))
        {
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape && SearchBox.IsFocused && DataContext is MainWindowViewModel viewModel)
        {
            viewModel.ClearSearchCommand.Execute(null);
            e.Handled = true;
        }
    }

    private void OnCommandPaletteKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Escape:
                viewModel.CloseCommandPaletteCommand.Execute(null);
                e.Handled = true;
                break;
            case Key.Down:
                viewModel.SelectNextCommandPaletteItem();
                e.Handled = true;
                break;
            case Key.Up:
                viewModel.SelectPreviousCommandPaletteItem();
                e.Handled = true;
                break;
            case Key.Enter:
                ExecuteSelectedCommandPaletteItem();
                e.Handled = true;
                break;
        }
    }

    private void OnCommandPaletteItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: CommandPaletteItemViewModel item }
            && DataContext is MainWindowViewModel viewModel)
        {
            ExecuteCommandPaletteItem(viewModel, item);
        }
    }

    private void ExecuteSelectedCommandPaletteItem()
    {
        if (DataContext is MainWindowViewModel viewModel && viewModel.SelectedCommandPaletteItem is not null)
        {
            ExecuteCommandPaletteItem(viewModel, viewModel.SelectedCommandPaletteItem);
        }
    }

    private void ExecuteCommandPaletteItem(MainWindowViewModel viewModel, CommandPaletteItemViewModel item)
    {
        if (viewModel.ExecuteCommandPaletteItem(item))
        {
            return;
        }

        ExecuteEditorCommandPaletteItem(viewModel, item);
        viewModel.CloseCommandPaletteCommand.Execute(null);
    }

    private void ExecuteEditorCommandPaletteItem(MainWindowViewModel viewModel, CommandPaletteItemViewModel item)
    {
        if (!viewModel.HasEditorVisible)
        {
            viewModel.ShowEditorCommand.Execute(null);
        }

        if (!TryGetFocusedMarkdownEditor(out var editor))
        {
            editor = MarkdownEditorSplit.IsVisible ? MarkdownEditorSplit : MarkdownEditor;
        }

        var formatType = item.ActionKind switch
        {
            CommandPaletteActionKind.Bold => "bold",
            CommandPaletteActionKind.Italic => "italic",
            CommandPaletteActionKind.Heading1 => "h1",
            CommandPaletteActionKind.Heading2 => "h2",
            CommandPaletteActionKind.Heading3 => "h3",
            CommandPaletteActionKind.List => "list",
            CommandPaletteActionKind.Checklist => "checklist",
            CommandPaletteActionKind.Quote => "quote",
            CommandPaletteActionKind.Code => "code",
            CommandPaletteActionKind.Link => "link",
            _ => null
        };

        if (formatType is not null)
        {
            ApplyToolbarFormat(formatType, editor);
            return;
        }

        var insertionType = item.ActionKind switch
        {
            CommandPaletteActionKind.InsertTable => "table",
            CommandPaletteActionKind.InsertCodeBlock => "code-block",
            CommandPaletteActionKind.InsertCallout => "callout",
            _ => null
        };

        if (insertionType is not null)
        {
            ApplyMarkdownInsertion(insertionType, editor);
        }
    }

    private bool TryHandleMarkdownShortcut(KeyEventArgs e)
    {
        if (!TryGetFocusedMarkdownEditor(out var editor))
        {
            return false;
        }

        var hasControl = e.KeyModifiers.HasFlag(KeyModifiers.Control);
        var hasAlt = e.KeyModifiers.HasFlag(KeyModifiers.Alt);
        var hasShift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);

        var formatType = e.Key switch
        {
            Key.B when hasControl && !hasAlt && !hasShift => "bold",
            Key.I when hasControl && !hasAlt && !hasShift => "italic",
            Key.D1 when hasControl && hasAlt && !hasShift => "h1",
            Key.D2 when hasControl && hasAlt && !hasShift => "h2",
            Key.D3 when hasControl && hasAlt && !hasShift => "h3",
            Key.D7 when hasControl && hasShift && !hasAlt => "list",
            Key.D8 when hasControl && hasShift && !hasAlt => "checklist",
            Key.Oem3 when hasControl && !hasAlt && !hasShift => "code",
            Key.Q when hasControl && hasShift && !hasAlt => "quote",
            _ => null
        };

        if (formatType is null)
        {
            return false;
        }

        ApplyToolbarFormat(formatType, editor);
        return true;
    }

    private void ApplyToolbarFormat(string type)
    {
        if (!TryGetFocusedMarkdownEditor(out var editor))
        {
            editor = MarkdownEditorSplit.IsVisible ? MarkdownEditorSplit : MarkdownEditor;
        }

        ApplyToolbarFormat(type, editor);
    }

    private void ApplyToolbarFormat(string type, TextBox editor)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var text = editor.Text ?? string.Empty;
        var selStart = editor.SelectionStart;
        var selEnd = editor.SelectionEnd;

        var (newText, newSelStart, newSelEnd) = viewModel.ApplyMarkdownFormat(text, selStart, selEnd, type);

        viewModel.Markdown = newText;
        editor.SelectionStart = newSelStart;
        editor.SelectionEnd = newSelEnd;
        editor.Focus();
    }

    private void ApplyMarkdownInsertion(string type, TextBox editor)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var text = editor.Text ?? string.Empty;
        var selStart = editor.SelectionStart;
        var selEnd = editor.SelectionEnd;
        var (newText, newSelStart, newSelEnd) = viewModel.ApplyMarkdownInsertion(text, selStart, selEnd, type);

        viewModel.Markdown = newText;
        editor.SelectionStart = newSelStart;
        editor.SelectionEnd = newSelEnd;
        editor.Focus();
    }

    private bool TryGetFocusedMarkdownEditor(out TextBox editor)
    {
        if (MarkdownEditor.IsFocused)
        {
            editor = MarkdownEditor;
            return true;
        }

        if (MarkdownEditorSplit.IsFocused)
        {
            editor = MarkdownEditorSplit;
            return true;
        }

        editor = MarkdownEditor;
        return false;
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

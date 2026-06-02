using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;
using Avalonia.VisualTree;
using RepoNotes.App.ViewModels;

namespace RepoNotes.App.Controls;

public sealed class MarkdownInlineTextBlock : TextBlock
{
    public static readonly StyledProperty<IEnumerable<MarkdownInlineRun>?> InlineRunsProperty =
        AvaloniaProperty.Register<MarkdownInlineTextBlock, IEnumerable<MarkdownInlineRun>?>(nameof(InlineRuns));

    public IEnumerable<MarkdownInlineRun>? InlineRuns
    {
        get => GetValue(InlineRunsProperty);
        set => SetValue(InlineRunsProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == InlineRunsProperty)
        {
            RebuildInlines();
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        RebuildInlines();
    }

    private void RebuildInlines()
    {
        Inlines ??= [];
        Inlines.Clear();

        if (InlineRuns is null)
        {
            return;
        }

        foreach (var inlineRun in InlineRuns)
        {
            if (string.IsNullOrEmpty(inlineRun.Text))
            {
                continue;
            }

            Inlines?.Add(CreateRun(inlineRun));
        }

        Text = string.Empty;
    }

    private static Run CreateRun(MarkdownInlineRun inlineRun)
    {
        var run = new Run(inlineRun.Text);

        if (inlineRun.IsBold)
        {
            run.FontWeight = FontWeight.Bold;
        }

        if (inlineRun.IsItalic)
        {
            run.FontStyle = FontStyle.Italic;
        }

        if (inlineRun.IsCode)
        {
            run.FontFamily = new FontFamily("Cascadia Mono, Consolas");
            run.Foreground = new SolidColorBrush(Color.Parse("#C4B5FD"));
        }

        if (inlineRun.IsLink)
        {
            run.Foreground = new SolidColorBrush(Color.Parse("#93C5FD"));
            run.TextDecorations = Avalonia.Media.TextDecorations.Underline;
        }

        return run;
    }
}

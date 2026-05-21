using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace RepoNotes.App.Services;

public sealed class AvaloniaTextPromptService(Window owner) : ITextPromptService
{
    public async Task<string?> PromptAsync(string title, string message, string initialValue)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 420,
            Height = 214,
            MinWidth = 420,
            MinHeight = 214,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Background = Brush.Parse("#0B111A"),
            Foreground = Brush.Parse("#E5E7EB"),
            FontFamily = new FontFamily("Inter, Segoe UI, Arial"),
            CanResize = false
        };

        var input = new TextBox
        {
            Text = initialValue,
            Height = 34,
            Background = Brush.Parse("#0B1018"),
            Foreground = Brush.Parse("#E5E7EB"),
            BorderBrush = Brush.Parse("#223041"),
            BorderThickness = new Avalonia.Thickness(1),
            CornerRadius = new Avalonia.CornerRadius(8),
            Padding = new Avalonia.Thickness(10, 5)
        };
        var errorText = new TextBlock
        {
            Foreground = Brush.Parse("#FCA5A5"),
            FontSize = 12,
            MinHeight = 18
        };

        var confirmButton = new Button
        {
            Content = "Confirmar",
            Height = 32,
            MinWidth = 92,
            Background = Brush.Parse("#6366F1"),
            BorderBrush = Brush.Parse("#6366F1"),
            Foreground = Brushes.White,
            CornerRadius = new Avalonia.CornerRadius(8),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        var cancelButton = new Button
        {
            Content = "Cancelar",
            Height = 32,
            MinWidth = 86,
            Background = Brush.Parse("#111B28"),
            BorderBrush = Brush.Parse("#223041"),
            Foreground = Brush.Parse("#E5E7EB"),
            CornerRadius = new Avalonia.CornerRadius(8),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center
        };

        confirmButton.Click += (_, _) =>
        {
            var value = input.Text?.Trim() ?? string.Empty;
            var hasInvalidCharacter = value.Any(Path.GetInvalidFileNameChars().Contains);

            if (string.IsNullOrWhiteSpace(value))
            {
                errorText.Text = "Informe um nome.";
                return;
            }

            if (hasInvalidCharacter)
            {
                errorText.Text = "O nome contem caracteres invalidos para Windows.";
                return;
            }

            dialog.Close(value);
        };
        cancelButton.Click += (_, _) => dialog.Close(null);

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 8,
            Children = { cancelButton, confirmButton }
        };

        var contentGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },
            Children =
            {
                new TextBlock
                {
                    Text = title,
                    FontSize = 18,
                    FontWeight = FontWeight.SemiBold,
                    Margin = new Avalonia.Thickness(0, 0, 0, 8)
                },
                new TextBlock
                {
                    Text = message,
                    Foreground = Brush.Parse("#9CA3AF"),
                    FontSize = 12,
                    Margin = new Avalonia.Thickness(0, 0, 0, 12)
                },
                input,
                errorText,
                buttonPanel
            }
        };

        Grid.SetRow(contentGrid.Children[1], 1);
        Grid.SetRow(input, 2);
        Grid.SetRow(errorText, 3);
        Grid.SetRow(buttonPanel, 4);

        dialog.Content = new Border
        {
            Padding = new Avalonia.Thickness(18),
            Background = Brush.Parse("#0B111A"),
            Child = contentGrid
        };

        input.SelectAll();
        return await dialog.ShowDialog<string?>(owner);
    }
}

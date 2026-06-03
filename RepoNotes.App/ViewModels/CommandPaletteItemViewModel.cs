namespace RepoNotes.App.ViewModels;

public sealed class CommandPaletteItemViewModel(
    string id,
    string title,
    string description,
    string shortcutText,
    CommandPaletteActionKind actionKind,
    bool requiresEditor = false) : ViewModelBase
{
    private bool _isSelected;

    public string Id { get; } = id;

    public string Title { get; } = title;

    public string Description { get; } = description;

    public string ShortcutText { get; } = shortcutText;

    public CommandPaletteActionKind ActionKind { get; } = actionKind;

    public bool RequiresEditor { get; } = requiresEditor;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public bool Matches(string query) =>
        string.IsNullOrWhiteSpace(query)
        || Title.Contains(query, StringComparison.OrdinalIgnoreCase)
        || Description.Contains(query, StringComparison.OrdinalIgnoreCase)
        || Id.Contains(query, StringComparison.OrdinalIgnoreCase)
        || ShortcutText.Contains(query, StringComparison.OrdinalIgnoreCase);
}

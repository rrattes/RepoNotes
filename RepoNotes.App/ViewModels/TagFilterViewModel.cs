using System.Windows.Input;

namespace RepoNotes.App.ViewModels;

public sealed class TagFilterViewModel(
    string name,
    int count,
    bool isActive,
    Action<string> selectTag) : ViewModelBase
{
    private bool _isActive = isActive;

    public string Name { get; } = name;

    public int Count { get; } = count;

    public string Label => $"{Name} {Count}";

    public ICommand SelectCommand { get; } = new RelayCommand(() => selectTag(name));

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }
}

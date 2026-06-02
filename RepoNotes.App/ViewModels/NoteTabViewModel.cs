using System.Windows.Input;
using RepoNotes.Core.Models;

namespace RepoNotes.App.ViewModels;

public sealed class NoteTabViewModel : ViewModelBase
{
    private readonly Action<NoteTabViewModel> _activate;
    private readonly Action<NoteTabViewModel> _close;
    private NoteItem _note;
    private bool _isActive;
    private bool _isDirty;
    private string _status = "Salvo";
    private string _lastErrorMessage = string.Empty;

    public NoteTabViewModel(NoteItem note, Action<NoteTabViewModel> activate, Action<NoteTabViewModel> close)
    {
        _note = note;
        _activate = activate;
        _close = close;
        ActivateCommand = new RelayCommand(() => _activate(this));
        CloseCommand = new RelayCommand(() => _close(this));
    }

    public NoteItem Note
    {
        get => _note;
        private set => SetProperty(ref _note, value);
    }

    public ICommand ActivateCommand { get; }

    public ICommand CloseCommand { get; }

    public string NoteId => Note.Id;

    public string Title => Note.Title;

    public string Path => Note.Path;

    public string Markdown => Note.Markdown;

    public DateTime UpdatedAt => Note.UpdatedAt;

    public DateTime CreatedAt => Note.CreatedAt;

    public IReadOnlyList<string> Tags => Note.Tags;

    public string Type => Note.Type;

    public string NoteStatus => Note.Status;

    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        set
        {
            if (SetProperty(ref _isDirty, value))
            {
                OnPropertyChanged(nameof(DirtyIndicator));
            }
        }
    }

    public string DirtyIndicator => IsDirty ? "*" : string.Empty;

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public string LastErrorMessage
    {
        get => _lastErrorMessage;
        set => SetProperty(ref _lastErrorMessage, value);
    }

    public void UpdateNote(NoteItem note)
    {
        Note = note;
        RefreshFromNote();
    }

    public void RefreshFromNote()
    {
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Path));
        OnPropertyChanged(nameof(Markdown));
        OnPropertyChanged(nameof(UpdatedAt));
        OnPropertyChanged(nameof(CreatedAt));
        OnPropertyChanged(nameof(Tags));
        OnPropertyChanged(nameof(Type));
        OnPropertyChanged(nameof(NoteStatus));
    }
}

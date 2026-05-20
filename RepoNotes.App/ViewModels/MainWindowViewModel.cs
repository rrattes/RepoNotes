using System.Collections.ObjectModel;
using System.Windows.Input;
using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.App.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly INoteRepository _noteRepository;
    private NoteItem? _selectedNote;
    private RepositoryNodeViewModel? _selectedNode;
    private string _searchText = string.Empty;
    private string _status = "Salvo";
    private string _lastErrorMessage = string.Empty;
    private bool _hasUnsavedChanges;

    public MainWindowViewModel(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
        RepositoryName = noteRepository.CurrentRepository.Name;
        RepositoryPath = noteRepository.CurrentRepository.RootPath;
        Nodes = new ObservableCollection<RepositoryNodeViewModel>(
            noteRepository.GetTree().Select(node => new RepositoryNodeViewModel(node)));

        NewNoteCommand = new RelayCommand(() => Status = "Nova nota pronta para implementacao");
        NewFolderCommand = new RelayCommand(() => Status = "Nova pasta pronta para implementacao");
        OpenSettingsCommand = new RelayCommand(() => Status = "Configuracoes ainda nao implementadas no MVP");
        SaveNoteCommand = new RelayCommand(SaveSelectedNote, () => SelectedNote is not null);

        SelectedNote = noteRepository.GetNotes().FirstOrDefault();
    }

    public string AppName => "RepoNotes";

    public string RepositoryName { get; }

    public string RepositoryPath { get; }

    public ObservableCollection<RepositoryNodeViewModel> Nodes { get; }

    public ICommand NewNoteCommand { get; }

    public ICommand NewFolderCommand { get; }

    public ICommand OpenSettingsCommand { get; }

    public ICommand SaveNoteCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public RepositoryNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        set
        {
            if (!SetProperty(ref _selectedNode, value) || value?.NoteId is null)
            {
                return;
            }

            SelectedNote = _noteRepository.GetNoteById(value.NoteId);
        }
    }

    public NoteItem? SelectedNote
    {
        get => _selectedNote;
        private set
        {
            if (!SetProperty(ref _selectedNote, value))
            {
                return;
            }

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Markdown));
            OnPropertyChanged(nameof(PreviewText));
            OnPropertyChanged(nameof(NotePath));
            OnPropertyChanged(nameof(WordCountText));
            OnPropertyChanged(nameof(UpdatedAtText));
            OnPropertyChanged(nameof(TagsText));
            _hasUnsavedChanges = false;
            LastErrorMessage = string.Empty;
            Status = "Salvo";
        }
    }

    public string Title
    {
        get => SelectedNote?.Title ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Title == value)
            {
                return;
            }

            SelectedNote.Title = value;
            MarkNoteChanged();
            OnPropertyChanged();
            OnPropertyChanged(nameof(PreviewText));
        }
    }

    public string Markdown
    {
        get => SelectedNote?.Markdown ?? string.Empty;
        set
        {
            if (SelectedNote is null || SelectedNote.Markdown == value)
            {
                return;
            }

            SelectedNote.Markdown = value;
            MarkNoteChanged();
            OnPropertyChanged();
            OnPropertyChanged(nameof(PreviewText));
            OnPropertyChanged(nameof(WordCountText));
        }
    }

    public string PreviewText => SelectedNote is null
        ? "Selecione uma nota para visualizar."
        : Markdown;

    public string NotePath => SelectedNote?.Path ?? RepositoryPath;

    public string WordCountText => $"{SelectedNote?.WordCount ?? 0} palavras";

    public string UpdatedAtText => SelectedNote is null
        ? "-"
        : SelectedNote.UpdatedAt.ToString("dd/MM/yyyy HH:mm");

    public string TagsText => SelectedNote is null || SelectedNote.Tags.Count == 0
        ? "Sem tags"
        : string.Join(", ", SelectedNote.Tags);

    public string Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public string LastErrorMessage
    {
        get => _lastErrorMessage;
        private set => SetProperty(ref _lastErrorMessage, value);
    }

    private void MarkNoteChanged()
    {
        if (SelectedNote is not null)
        {
            SelectedNote.UpdatedAt = DateTime.Now;
        }

        Status = "Alterado";
        LastErrorMessage = string.Empty;
        _hasUnsavedChanges = true;
        OnPropertyChanged(nameof(UpdatedAtText));
    }

    private void SaveSelectedNote()
    {
        if (SelectedNote is null || !_hasUnsavedChanges)
        {
            Status = "Salvo";
            LastErrorMessage = string.Empty;
            return;
        }

        try
        {
            Status = "Salvando...";
            LastErrorMessage = string.Empty;
            _noteRepository.SaveNote(SelectedNote);
            _hasUnsavedChanges = false;
            Status = "Salvo";
            OnPropertyChanged(nameof(UpdatedAtText));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            LastErrorMessage = ex.Message;
            Status = "Erro ao salvar";
        }
    }
}

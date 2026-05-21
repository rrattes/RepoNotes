using System.Windows.Input;
using RepoNotes.App.Services;

namespace RepoNotes.App.ViewModels;

public sealed class InternalLinkViewModel
{
    private readonly Action<InternalLinkViewModel> _openLink;

    public InternalLinkViewModel(InternalLinkResult link, Action<InternalLinkViewModel> openLink)
    {
        _openLink = openLink;
        Target = link.Target;
        DisplayText = link.DisplayText;
        NoteId = link.NoteId;
        NotePath = link.NotePath;
        IsResolved = link.IsResolved;
        OpenCommand = new RelayCommand(Open);
    }

    public string Target { get; }

    public string DisplayText { get; }

    public string? NoteId { get; }

    public string? NotePath { get; }

    public bool IsResolved { get; }

    public string StatusText => IsResolved ? "Resolvido" : "Quebrado";

    public string TooltipText => IsResolved
        ? $"Abrir {NotePath}"
        : "Link interno nao encontrado";

    public ICommand OpenCommand { get; }

    private void Open() => _openLink(this);
}

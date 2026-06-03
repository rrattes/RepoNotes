using System.Windows.Input;

namespace RepoNotes.App.ViewModels;

public sealed class ParameterizedRelayCommand<T>(Action<T?> execute, Func<T?, bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) =>
        parameter is null
            ? canExecute?.Invoke(default) ?? true
            : parameter is T typedParameter && (canExecute?.Invoke(typedParameter) ?? true);

    public void Execute(object? parameter)
    {
        if (parameter is null)
        {
            execute(default);
            return;
        }

        if (parameter is T typedParameter)
        {
            execute(typedParameter);
        }
    }

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

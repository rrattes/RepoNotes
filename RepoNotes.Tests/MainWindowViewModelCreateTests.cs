using RepoNotes.App.Services;
using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelCreateTests : IDisposable
{
    private readonly string _tempRepositoryPath;

    public MainWindowViewModelCreateTests()
    {
        _tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempRepositoryPath);
    }

    [Fact]
    public void NewNoteCommandCreatesFileAndOpensItInEditor()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.NewNoteCommand.Execute(null);

        Assert.Equal("Nova nota.md", viewModel.NotePath);
        Assert.Equal("Nova nota", viewModel.Title);
        Assert.Equal("Nota criada: Nova nota.md", viewModel.Status);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota.md")));
    }

    [Fact]
    public void NewNoteCommandDoesNotCreateFileWhenNamePromptIsCanceled()
    {
        var viewModel = new MainWindowViewModel(
            new LocalMarkdownNoteRepository(_tempRepositoryPath),
            textPromptService: new TestTextPromptService((string?)null));

        viewModel.NewNoteCommand.Execute(null);

        Assert.Equal("Criacao cancelada", viewModel.Status);
        Assert.False(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota.md")));
    }

    [Fact]
    public void NewNoteCommandUsesPromptedNameAndSanitizesInvalidCharacters()
    {
        var viewModel = new MainWindowViewModel(
            new LocalMarkdownNoteRepository(_tempRepositoryPath),
            textPromptService: new TestTextPromptService("Nota:Invalida"));

        viewModel.NewNoteCommand.Execute(null);

        Assert.Equal("Nota-Invalida.md", viewModel.NotePath);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nota-Invalida.md")));
    }

    [Fact]
    public void NewFromTemplateCommandCreatesSelectedTemplateFileAndOpensItInEditor()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));
        viewModel.SelectedTemplate = viewModel.Templates.First(template => template.Id == "runbook");

        viewModel.NewFromTemplateCommand.Execute(null);

        Assert.Equal("Novo runbook.md", viewModel.NotePath);
        Assert.Equal("Novo runbook", viewModel.Title);
        Assert.Equal("Nota criada por template: Novo runbook.md", viewModel.Status);
        Assert.Contains("## Pre-requisitos", viewModel.Markdown);
        Assert.Contains("## Rollback", viewModel.Markdown);

        var savedContent = File.ReadAllText(Path.Combine(_tempRepositoryPath, "Novo runbook.md"));
        Assert.Contains("type: runbook", savedContent);
        Assert.Contains("tags: [runbook, operacao]", savedContent);
    }

    [Fact]
    public void NewFromTemplateCommandUsesUniqueNamesForRepeatedTemplateCreation()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));
        viewModel.SelectedTemplate = viewModel.Templates.First(template => template.Id == "runbook");

        viewModel.NewFromTemplateCommand.Execute(null);
        viewModel.NewFromTemplateCommand.Execute(null);

        Assert.Equal("Novo runbook 2.md", viewModel.NotePath);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Novo runbook.md")));
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Novo runbook 2.md")));
    }

    [Fact]
    public void NewFolderCommandCreatesFolderAndRefreshesTree()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.NewFolderCommand.Execute(null);

        Assert.Equal("Pasta criada: Nova pasta", viewModel.Status);
        Assert.True(Directory.Exists(Path.Combine(_tempRepositoryPath, "Nova pasta")));
        Assert.Contains(viewModel.Nodes, node => node.Path == "Nova pasta");
    }

    [Fact]
    public void RenameSelectedItemCommandRenamesOpenNoteUsingTitle()
    {
        var viewModel = new MainWindowViewModel(
            new LocalMarkdownNoteRepository(_tempRepositoryPath),
            textPromptService: new TestTextPromptService("Nova nota", "Nota Renomeada"));
        viewModel.NewNoteCommand.Execute(null);

        viewModel.RenameSelectedItemCommand.Execute(null);

        Assert.Equal("Nota Renomeada.md", viewModel.NotePath);
        Assert.Equal("Item renomeado: Nota Renomeada.md", viewModel.Status);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Nota Renomeada.md")));
        Assert.False(File.Exists(Path.Combine(_tempRepositoryPath, "Nova nota.md")));
    }

    [Fact]
    public void RenameSelectedItemCommandDoesNotOverwriteExistingFileOnConflict()
    {
        var repository = new LocalMarkdownNoteRepository(_tempRepositoryPath);
        var viewModel = new MainWindowViewModel(repository, textPromptService: new TestTextPromptService("Nova nota", "Conflito"));
        viewModel.NewNoteCommand.Execute(null);
        File.WriteAllText(Path.Combine(_tempRepositoryPath, "Conflito.md"), "# Existente");

        viewModel.RenameSelectedItemCommand.Execute(null);

        Assert.Equal("Conflito 2.md", viewModel.NotePath);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Conflito.md")));
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, "Conflito 2.md")));
    }

    [Fact]
    public void DeleteSelectedItemCommandMovesOpenNoteToTrashAndKeepsEditorUsable()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));
        viewModel.NewNoteCommand.Execute(null);

        viewModel.DeleteSelectedItemCommand.Execute(null);

        Assert.StartsWith("Item movido para a lixeira:", viewModel.Status);
        Assert.True(File.Exists(Path.Combine(_tempRepositoryPath, ".reponotes-trash", "Nova nota.md")));
        Assert.NotEqual("Nova nota.md", viewModel.NotePath);
    }

    [Fact]
    public void DeleteSelectedItemCommandDoesNotDeleteRepositoryRoot()
    {
        var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(_tempRepositoryPath));

        viewModel.DeleteSelectedItemCommand.Execute(null);

        Assert.Equal("Raiz do repositorio nao pode ser excluida", viewModel.Status);
        Assert.False(Directory.Exists(Path.Combine(_tempRepositoryPath, ".reponotes-trash")));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRepositoryPath))
        {
            Directory.Delete(_tempRepositoryPath, recursive: true);
        }
    }

    private sealed class TestTextPromptService(params string?[] values) : ITextPromptService
    {
        private readonly Queue<string?> _values = new(values);

        public Task<string?> PromptAsync(string title, string message, string initialValue) =>
            Task.FromResult(_values.Count == 0 ? initialValue : _values.Dequeue());
    }
}

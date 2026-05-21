using RepoNotes.App.ViewModels;
using RepoNotes.App.Services;
using RepoNotes.Core.Models;
using RepoNotes.Core.Services;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelSaveTests
{
    [Fact]
    public void SaveCommandDoesNotWriteWhenNoteIsUnchanged()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository);

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(0, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal(string.Empty, viewModel.LastErrorMessage);
    }

    [Fact]
    public void SaveCommandMarksChangedNoteAsSaved()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Nota atualizada"
        };

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal(string.Empty, viewModel.LastErrorMessage);
    }

    [Fact]
    public void SaveCommandKeepsEditedContentAndShowsErrorWhenSaveFails()
    {
        var repository = new TestNoteRepository
        {
            SaveException = new IOException("Arquivo bloqueado para teste.")
        };
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Conteudo que nao deve ser perdido"
        };

        viewModel.SaveNoteCommand.Execute(null);

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Erro ao salvar", viewModel.Status);
        Assert.Equal("Arquivo bloqueado para teste.", viewModel.LastErrorMessage);
        Assert.Equal("# Conteudo que nao deve ser perdido", viewModel.Markdown);
    }

    [Fact]
    public void SaveCommandWritesEditedTypeTagsAndStatusToFrontmatter()
    {
        var tempRepositoryPath = CreateTempRepository();
        try
        {
            var notePath = Path.Combine(tempRepositoryPath, "Nota.md");
            File.WriteAllText(notePath, """
            # Nota

            Corpo original.
            """);
            var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(tempRepositoryPath))
            {
                NoteType = "runbook",
                MetadataTagsText = "infra, windows, infra",
                NoteStatus = "Review"
            };

            viewModel.SaveNoteCommand.Execute(null);

            var savedContent = File.ReadAllText(notePath);
            Assert.Contains("type: runbook", savedContent);
            Assert.Contains("tags: [infra, windows]", savedContent);
            Assert.Contains("status: Review", savedContent);
            Assert.Contains("# Nota", savedContent);
            Assert.Contains("Corpo original.", savedContent);
            Assert.Equal("Salvo", viewModel.Status);
        }
        finally
        {
            DeleteTempRepository(tempRepositoryPath);
        }
    }

    [Fact]
    public void SaveCommandCreatesFrontmatterForNoteWithoutFrontmatter()
    {
        var tempRepositoryPath = CreateTempRepository();
        try
        {
            var notePath = Path.Combine(tempRepositoryPath, "SemFrontmatter.md");
            File.WriteAllText(notePath, """
            # Nota sem frontmatter

            Conteudo Markdown preservado.
            """);
            var viewModel = new MainWindowViewModel(new LocalMarkdownNoteRepository(tempRepositoryPath))
            {
                NoteType = "script",
                MetadataTagsText = "powershell, automacao",
                NoteStatus = "Active"
            };

            viewModel.SaveNoteCommand.Execute(null);

            var savedContent = File.ReadAllText(notePath);
            Assert.StartsWith("---", savedContent);
            Assert.Contains("title: Nota sem frontmatter", savedContent);
            Assert.Contains("type: script", savedContent);
            Assert.Contains("tags: [powershell, automacao]", savedContent);
            Assert.Contains("status: Active", savedContent);
            Assert.Contains("# Nota sem frontmatter", savedContent);
            Assert.Contains("Conteudo Markdown preservado.", savedContent);
        }
        finally
        {
            DeleteTempRepository(tempRepositoryPath);
        }
    }

    [Fact]
    public void SelectingAnotherNoteSwitchesWithoutSavingWhenCurrentNoteIsUnchanged()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository);

        viewModel.SelectedNode = viewModel.Nodes[1];

        Assert.Equal(0, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal("note-2", viewModel.SelectedNode?.NoteId);
        Assert.Equal("# Segunda nota", viewModel.Markdown);
    }

    [Fact]
    public void SelectingAnotherNoteSavesChangedCurrentNoteBeforeSwitching()
    {
        var repository = new TestNoteRepository();
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Conteudo salvo antes da troca"
        };

        viewModel.SelectedNode = viewModel.Nodes[1];

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Salvo", viewModel.Status);
        Assert.Equal(string.Empty, viewModel.LastErrorMessage);
        Assert.Equal("note-2", viewModel.SelectedNode?.NoteId);
        Assert.Equal("# Segunda nota", viewModel.Markdown);
        Assert.Equal("# Conteudo salvo antes da troca", repository.FirstNote.Markdown);
    }

    [Fact]
    public void SelectingAnotherNoteKeepsCurrentNoteOpenWhenSaveFails()
    {
        var repository = new TestNoteRepository
        {
            SaveException = new IOException("Nao foi possivel salvar antes da troca.")
        };
        var viewModel = new MainWindowViewModel(repository)
        {
            Markdown = "# Conteudo ainda aberto"
        };

        viewModel.SelectedNode = viewModel.Nodes[1];

        Assert.Equal(1, repository.SaveCount);
        Assert.Equal("Erro ao salvar", viewModel.Status);
        Assert.Equal("Nao foi possivel salvar antes da troca.", viewModel.LastErrorMessage);
        Assert.Null(viewModel.SelectedNode);
        Assert.Equal("note", repository.FirstNote.Id);
        Assert.Equal("# Conteudo ainda aberto", viewModel.Markdown);
    }

    [Fact]
    public void OpenRepositoryCommandLoadsSelectedFolderAndPersistsIt()
    {
        var selectedPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(selectedPath);
        var settingsStore = new TestRepositorySettingsStore();
        var viewModel = new MainWindowViewModel(
            new TestNoteRepository("Repositorio inicial", "inicial"),
            new TestFolderPickerService(selectedPath),
            settingsStore,
            path => new TestNoteRepository(Path.GetFileName(path!), path!));

        viewModel.OpenRepositoryCommand.Execute(null);

        Assert.Equal(Path.GetFileName(selectedPath), viewModel.RepositoryName);
        Assert.Equal(Path.GetFullPath(selectedPath), settingsStore.LastRepositoryPath);
        Assert.Equal("Repositorio aberto: " + Path.GetFileName(selectedPath), viewModel.Status);
        Assert.Equal("# Nota", viewModel.Markdown);

        Directory.Delete(selectedPath, recursive: true);
    }

    [Fact]
    public void OpenRepositoryCommandFallsBackWhenSelectedFolderDoesNotExist()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        var settingsStore = new TestRepositorySettingsStore();
        var viewModel = new MainWindowViewModel(
            new TestNoteRepository("Repositorio inicial", "inicial"),
            new TestFolderPickerService(missingPath),
            settingsStore,
            path => new TestNoteRepository(path is null ? "sample-repository" : Path.GetFileName(path), path ?? "sample-repository"));

        viewModel.OpenRepositoryCommand.Execute(null);

        Assert.Equal("sample-repository", viewModel.RepositoryName);
        Assert.Null(settingsStore.LastRepositoryPath);
        Assert.Equal("Repositorio nao encontrado. Usando sample-repository.", viewModel.Status);
    }

    private static string CreateTempRepository()
    {
        var tempRepositoryPath = Path.Combine(Path.GetTempPath(), "RepoNotes.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRepositoryPath);
        return tempRepositoryPath;
    }

    private static void DeleteTempRepository(string tempRepositoryPath)
    {
        if (Directory.Exists(tempRepositoryPath))
        {
            Directory.Delete(tempRepositoryPath, recursive: true);
        }
    }

    private sealed class TestNoteRepository : INoteRepository
    {
        private readonly NoteItem _firstNote = new()
        {
            Id = "note",
            Title = "Nota",
            Markdown = "# Nota",
            Path = "Nota.md",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        private readonly NoteItem _secondNote = new()
        {
            Id = "note-2",
            Title = "Segunda nota",
            Markdown = "# Segunda nota",
            Path = "Segunda.md",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        public TestNoteRepository(string repositoryName = "Test Repository", string rootPath = "test")
        {
            CurrentRepository = new RepositoryItem
            {
                Id = "test",
                Name = repositoryName,
                RootPath = Path.GetFullPath(rootPath)
            };
        }

        public RepositoryItem CurrentRepository { get; }

        public int SaveCount { get; private set; }

        public Exception? SaveException { get; init; }

        public NoteItem FirstNote => _firstNote;

        private readonly List<NoteItem> _createdNotes = [];

        private readonly List<RepositoryNode> _createdNodes = [];

        public IReadOnlyList<RepositoryNode> GetTree()
        {
            var nodes = new List<RepositoryNode>
            {
                new()
                {
                    Name = "Nota.md",
                    Type = RepositoryNodeType.Note,
                    NoteId = "note",
                    Path = "Nota.md"
                },
                new()
                {
                    Name = "Segunda.md",
                    Type = RepositoryNodeType.Note,
                    NoteId = "note-2",
                    Path = "Segunda.md"
                }
            };
            nodes.AddRange(_createdNodes);
            return nodes;
        }

        public IReadOnlyList<NoteItem> GetNotes()
        {
            var notes = new List<NoteItem> { _firstNote, _secondNote };
            notes.AddRange(_createdNotes);
            return notes;
        }

        public NoteItem? GetNoteById(string noteId) => noteId switch
        {
            "note" => _firstNote,
            "note-2" => _secondNote,
            _ => _createdNotes.FirstOrDefault(note => note.Id == noteId)
        };

        public void SaveNote(NoteItem note)
        {
            SaveCount++;
            if (SaveException is not null)
            {
                throw SaveException;
            }
        }

        public NoteItem CreateNote(string? folderPath, string noteName = "Nova nota", NoteTemplate? template = null)
        {
            var fileName = $"{noteName}.md";
            var path = string.IsNullOrWhiteSpace(folderPath) ? fileName : @$"{folderPath}\{fileName}";
            var note = new NoteItem
            {
                Id = path,
                Title = noteName,
                Markdown = template?.CreateMarkdown(noteName) ?? $"# {noteName}{Environment.NewLine}",
                Path = path,
                Type = template?.SuggestedType ?? "note",
                Tags = template?.SuggestedTags ?? [],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _createdNotes.Add(note);
            _createdNodes.Add(new RepositoryNode
            {
                Name = fileName,
                Type = RepositoryNodeType.Note,
                NoteId = note.Id,
                Path = path
            });

            return note;
        }

        public string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta")
        {
            var path = string.IsNullOrWhiteSpace(parentFolderPath) ? folderName : @$"{parentFolderPath}\{folderName}";
            _createdNodes.Add(new RepositoryNode
            {
                Name = folderName,
                Type = RepositoryNodeType.Folder,
                Path = path
            });

            return path;
        }

        public string RenameItem(string itemPath, string newName)
        {
            if (string.IsNullOrWhiteSpace(itemPath))
            {
                throw new InvalidOperationException("Repository root cannot be renamed.");
            }

            return itemPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                ? $"{newName}.md"
                : newName;
        }

        public string MoveItemToTrash(string itemPath)
        {
            if (string.IsNullOrWhiteSpace(itemPath))
            {
                throw new InvalidOperationException("Repository root cannot be deleted.");
            }

            return @$".reponotes-trash\{Path.GetFileName(itemPath)}";
        }
    }

    private sealed class TestFolderPickerService(string? selectedPath) : IFolderPickerService
    {
        public Task<string?> PickRepositoryPathAsync() =>
            Task.FromResult(selectedPath);
    }

    private sealed class TestRepositorySettingsStore : IRepositorySettingsStore
    {
        public string? LastRepositoryPath { get; private set; }

        public string? GetLastRepositoryPath() => LastRepositoryPath;

        public void SaveLastRepositoryPath(string repositoryPath)
        {
            LastRepositoryPath = repositoryPath;
        }
    }
}

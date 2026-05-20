using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.Storage;

public sealed class MockNoteRepository : INoteRepository
{
    private readonly List<NoteItem> _notes;
    private readonly List<RepositoryNode> _tree;

    public MockNoteRepository()
    {
        CurrentRepository = new RepositoryItem
        {
            Id = "repo-local",
            Name = "Meu Repositorio Local",
            RootPath = @"C:\RepoNotes\MeuRepositorio"
        };

        var now = DateTime.Now;

        _notes =
        [
            new NoteItem
            {
                Id = "welcome",
                Title = "Bem-vindo ao RepoNotes",
                Path = @"Inbox\Bem-vindo.md",
                CreatedAt = now.AddDays(-3),
                UpdatedAt = now.AddMinutes(-18),
                Tags = ["mvp", "markdown"],
                Markdown = """
                # Bem-vindo ao RepoNotes

                Este e um editor local-first baseado em repositorios.

                - Escreva notas em Markdown.
                - Organize conhecimento em pastas.
                - Mantenha tudo em arquivos locais.

                O preview inicial ainda e simples, mas ja mostra o texto da nota selecionada.
                """
            },
            new NoteItem
            {
                Id = "roadmap",
                Title = "Roadmap do MVP",
                Path = @"Projetos\Roadmap.md",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddHours(-4),
                Tags = ["produto"],
                Markdown = """
                # Roadmap do MVP

                ## Agora
                Criar a estrutura do app, dados mockados e interface em tres colunas.

                ## Depois
                Persistencia em arquivos Markdown, busca real e renderizacao Markdown completa.
                """
            },
            new NoteItem
            {
                Id = "daily",
                Title = "Notas diarias",
                Path = @"Diario\2026-05-20.md",
                CreatedAt = now.Date,
                UpdatedAt = now.AddMinutes(-5),
                Tags = ["diario"],
                Markdown = """
                # 2026-05-20

                - Definir o layout principal.
                - Validar build.
                - Separar dominio, storage e app.
                """
            }
        ];

        _tree =
        [
            Folder("Inbox", @"Inbox", Note("Bem-vindo ao RepoNotes.md", "welcome", @"Inbox\Bem-vindo.md")),
            Folder("Projetos", @"Projetos", Note("Roadmap.md", "roadmap", @"Projetos\Roadmap.md")),
            Folder("Diario", @"Diario", Note("2026-05-20.md", "daily", @"Diario\2026-05-20.md"))
        ];
    }

    public RepositoryItem CurrentRepository { get; }

    public IReadOnlyList<RepositoryNode> GetTree() => _tree;

    public IReadOnlyList<NoteItem> GetNotes() => _notes;

    public NoteItem? GetNoteById(string noteId) =>
        _notes.FirstOrDefault(note => note.Id == noteId);

    public void SaveNote(NoteItem note)
    {
        note.UpdatedAt = DateTime.Now;
    }

    public NoteItem CreateNote(string? folderPath, string noteName = "Nova nota")
    {
        var now = DateTime.Now;
        var safeName = string.IsNullOrWhiteSpace(noteName) ? "Nova nota" : noteName;
        var fileName = safeName.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ? safeName : $"{safeName}.md";
        var path = string.IsNullOrWhiteSpace(folderPath) ? fileName : @$"{folderPath}\{fileName}";
        var note = new NoteItem
        {
            Id = path,
            Title = Path.GetFileNameWithoutExtension(fileName),
            Path = path,
            CreatedAt = now,
            UpdatedAt = now,
            Markdown = $"# {Path.GetFileNameWithoutExtension(fileName)}{Environment.NewLine}"
        };

        _notes.Add(note);
        _tree.Add(Note(fileName, note.Id, note.Path));
        return note;
    }

    public string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta")
    {
        var path = string.IsNullOrWhiteSpace(parentFolderPath) ? folderName : @$"{parentFolderPath}\{folderName}";
        _tree.Add(Folder(folderName, path));
        return path;
    }

    private static RepositoryNode Folder(string name, string path, params RepositoryNode[] children) =>
        new()
        {
            Name = name,
            Type = RepositoryNodeType.Folder,
            Path = path,
            Children = children
        };

    private static RepositoryNode Note(string name, string noteId, string path) =>
        new()
        {
            Name = name,
            Type = RepositoryNodeType.Note,
            NoteId = noteId,
            Path = path
        };
}

using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.Storage;

public sealed class LocalMarkdownNoteRepository : INoteRepository
{
    private readonly string _rootPath;
    private readonly List<NoteItem> _notes = [];
    private readonly List<RepositoryNode> _tree = [];

    public LocalMarkdownNoteRepository(string? rootPath = null)
    {
        _rootPath = ResolveRepositoryPath(rootPath);
        Directory.CreateDirectory(_rootPath);
        EnsureSampleRepository();

        CurrentRepository = new RepositoryItem
        {
            Id = "local-markdown",
            Name = new DirectoryInfo(_rootPath).Name,
            RootPath = _rootPath
        };

        Reload();
    }

    public RepositoryItem CurrentRepository { get; }

    public IReadOnlyList<RepositoryNode> GetTree() => _tree;

    public IReadOnlyList<NoteItem> GetNotes() => _notes;

    public NoteItem? GetNoteById(string noteId) =>
        _notes.FirstOrDefault(note => note.Id == noteId);

    public void SaveNote(NoteItem note)
    {
        var fullPath = GetSafeFullPath(note.Path);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, note.Markdown);

        var updatedAt = File.GetLastWriteTime(fullPath);
        note.UpdatedAt = updatedAt;

        var existing = _notes.FirstOrDefault(candidate => candidate.Id == note.Id);
        if (existing is not null && !ReferenceEquals(existing, note))
        {
            existing.Title = note.Title;
            existing.Markdown = note.Markdown;
            existing.UpdatedAt = updatedAt;
        }
    }

    public NoteItem CreateNote(string? folderPath, string noteName = "Nova nota")
    {
        var safeFolderPath = NormalizeOptionalPath(folderPath);
        var safeNoteName = SanitizeFileName(Path.GetFileNameWithoutExtension(noteName), "Nova nota");
        var targetDirectory = GetSafeDirectoryPath(safeFolderPath);
        Directory.CreateDirectory(targetDirectory);

        var fileName = GetUniqueFileName(targetDirectory, safeNoteName, ".md");
        var fullPath = Path.Combine(targetDirectory, fileName);
        var title = Path.GetFileNameWithoutExtension(fileName);
        var markdown = $"# {title}{Environment.NewLine}";

        File.WriteAllText(fullPath, markdown);
        var createdNote = CreateNote(fullPath);
        Reload();

        return GetNoteById(createdNote.Id) ?? createdNote;
    }

    public string CreateFolder(string? parentFolderPath, string folderName = "Nova pasta")
    {
        var safeParentPath = NormalizeOptionalPath(parentFolderPath);
        var safeFolderName = SanitizeFileName(folderName, "Nova pasta");
        var parentDirectory = GetSafeDirectoryPath(safeParentPath);
        Directory.CreateDirectory(parentDirectory);

        var directoryName = GetUniqueDirectoryName(parentDirectory, safeFolderName);
        var fullPath = Path.Combine(parentDirectory, directoryName);
        Directory.CreateDirectory(fullPath);
        Reload();

        return NormalizePath(Path.GetRelativePath(_rootPath, fullPath));
    }

    private void Reload()
    {
        _notes.Clear();
        _tree.Clear();

        foreach (var filePath in Directory.EnumerateFiles(_rootPath, "*.md", SearchOption.AllDirectories).OrderBy(path => path))
        {
            _notes.Add(CreateNote(filePath));
        }

        foreach (var node in BuildTree(_rootPath))
        {
            _tree.Add(node);
        }
    }

    private NoteItem CreateNote(string filePath)
    {
        var markdown = File.ReadAllText(filePath);
        var relativePath = Path.GetRelativePath(_rootPath, filePath);
        var normalizedPath = NormalizePath(relativePath);

        return new NoteItem
        {
            Id = normalizedPath,
            Title = GetTitle(markdown, filePath),
            Markdown = markdown,
            Path = normalizedPath,
            CreatedAt = File.GetCreationTime(filePath),
            UpdatedAt = File.GetLastWriteTime(filePath)
        };
    }

    private IReadOnlyList<RepositoryNode> BuildTree(string directoryPath)
    {
        var children = new List<RepositoryNode>();

        foreach (var childDirectory in Directory.EnumerateDirectories(directoryPath).OrderBy(path => path))
        {
            var nestedChildren = BuildTree(childDirectory);
            children.Add(new RepositoryNode
            {
                Name = Path.GetFileName(childDirectory),
                Type = RepositoryNodeType.Folder,
                Path = NormalizePath(Path.GetRelativePath(_rootPath, childDirectory)),
                Children = nestedChildren
            });
        }

        foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*.md").OrderBy(path => path))
        {
            var relativePath = NormalizePath(Path.GetRelativePath(_rootPath, filePath));
            children.Add(new RepositoryNode
            {
                Name = Path.GetFileName(filePath),
                Type = RepositoryNodeType.Note,
                NoteId = relativePath,
                Path = relativePath
            });
        }

        return children;
    }

    private string GetSafeFullPath(string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, relativePath));
        var rootPath = Path.GetFullPath(_rootPath);

        if (!fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Note path is outside the configured repository.");
        }

        return fullPath;
    }

    private string GetSafeDirectoryPath(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Path.GetFullPath(_rootPath);
        }

        var fullPath = GetSafeFullPath(relativePath);
        if (File.Exists(fullPath))
        {
            fullPath = Path.GetDirectoryName(fullPath)!;
        }

        return fullPath;
    }

    private static string? NormalizeOptionalPath(string? path) =>
        string.IsNullOrWhiteSpace(path) ? null : NormalizePath(path);

    private static string SanitizeFileName(string value, string fallback)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(value
            .Trim()
            .Select(character => invalidChars.Contains(character) ? '-' : character)
            .ToArray())
            .Trim(' ', '.');

        return string.IsNullOrWhiteSpace(sanitized) ? fallback : sanitized;
    }

    private static string GetUniqueFileName(string directoryPath, string baseName, string extension)
    {
        var candidate = $"{baseName}{extension}";
        var index = 2;

        while (File.Exists(Path.Combine(directoryPath, candidate)))
        {
            candidate = $"{baseName} {index}{extension}";
            index++;
        }

        return candidate;
    }

    private static string GetUniqueDirectoryName(string parentDirectoryPath, string baseName)
    {
        var candidate = baseName;
        var index = 2;

        while (Directory.Exists(Path.Combine(parentDirectoryPath, candidate)))
        {
            candidate = $"{baseName} {index}";
            index++;
        }

        return candidate;
    }

    private void EnsureSampleRepository()
    {
        if (Directory.EnumerateFiles(_rootPath, "*.md", SearchOption.AllDirectories).Any())
        {
            return;
        }

        WriteSample("Inbox", "Bem-vindo.md", """
        # Bem-vindo ao RepoNotes

        Este e um editor local-first baseado em repositorios.

        - Escreva notas em Markdown.
        - Organize conhecimento em pastas.
        - Mantenha tudo em arquivos locais.

        O preview inicial ainda e simples, mas ja mostra o texto da nota selecionada.
        """);

        WriteSample("Projetos", "Roadmap.md", """
        # Roadmap do MVP

        ## Agora
        Carregar arquivos Markdown locais, editar uma nota selecionada e salvar de volta no repositorio.

        ## Depois
        Melhorar navegacao de arquivos, busca local e preview Markdown real.
        """);

        WriteSample("Runbooks", "Deploy-local.md", """
        # Deploy local

        ## Checklist

        - Rodar build.
        - Rodar testes.
        - Conferir status do Git.
        - Publicar commit pequeno e verificavel.
        """);
    }

    private void WriteSample(string directoryName, string fileName, string content)
    {
        var directoryPath = Path.Combine(_rootPath, directoryName);
        Directory.CreateDirectory(directoryPath);
        File.WriteAllText(Path.Combine(directoryPath, fileName), content);
    }

    private static string ResolveRepositoryPath(string? rootPath)
    {
        if (!string.IsNullOrWhiteSpace(rootPath))
        {
            return Path.GetFullPath(rootPath);
        }

        var workspaceSamplePath = Path.Combine(Directory.GetCurrentDirectory(), "sample-repository");
        try
        {
            Directory.CreateDirectory(workspaceSamplePath);
            return Path.GetFullPath(workspaceSamplePath);
        }
        catch (IOException)
        {
            return GetLocalAppDataSamplePath();
        }
        catch (UnauthorizedAccessException)
        {
            return GetLocalAppDataSamplePath();
        }
    }

    private static string GetLocalAppDataSamplePath()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(localAppData, "RepoNotes", "sample-repository");
    }

    private static string NormalizePath(string path) =>
        path.Replace(Path.DirectorySeparatorChar, '\\').Replace(Path.AltDirectorySeparatorChar, '\\');

    private static string GetTitle(string markdown, string filePath)
    {
        var heading = markdown
            .Split(["\r\n", "\n"], StringSplitOptions.None)
            .Select(line => line.Trim())
            .FirstOrDefault(line => line.StartsWith("# ", StringComparison.Ordinal));

        return string.IsNullOrWhiteSpace(heading)
            ? Path.GetFileNameWithoutExtension(filePath)
            : heading[2..].Trim();
    }
}

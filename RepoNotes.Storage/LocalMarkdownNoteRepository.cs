using RepoNotes.Core.Models;
using RepoNotes.Core.Services;

namespace RepoNotes.Storage;

public sealed class LocalMarkdownNoteRepository : INoteRepository
{
    private const string TrashDirectoryName = ".reponotes-trash";

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
        note.UpdatedAt = DateTime.Now;
        File.WriteAllText(fullPath, ComposeMarkdownFile(note));

        var updatedAt = File.GetLastWriteTime(fullPath);
        note.UpdatedAt = updatedAt;

        var existing = _notes.FirstOrDefault(candidate => candidate.Id == note.Id);
        if (existing is not null && !ReferenceEquals(existing, note))
        {
            existing.Title = note.Title;
            existing.Markdown = note.Markdown;
            existing.Type = note.Type;
            existing.Status = note.Status;
            existing.Tags = note.Tags;
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
        var note = new NoteItem
        {
            Id = NormalizePath(Path.GetRelativePath(_rootPath, fullPath)),
            Title = title,
            Markdown = $"# {title}{Environment.NewLine}",
            Path = NormalizePath(Path.GetRelativePath(_rootPath, fullPath)),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        File.WriteAllText(fullPath, ComposeMarkdownFile(note));
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

    public string RenameItem(string itemPath, string newName)
    {
        if (string.IsNullOrWhiteSpace(itemPath))
        {
            throw new InvalidOperationException("Repository root cannot be renamed.");
        }

        var fullPath = GetSafeFullPath(itemPath);
        if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
        {
            throw new InvalidOperationException("Selected item does not exist.");
        }

        if (IsTrashPath(fullPath))
        {
            throw new InvalidOperationException("Trash items cannot be renamed from the main tree.");
        }

        var parentDirectory = Path.GetDirectoryName(fullPath)!;
        string targetPath;

        if (File.Exists(fullPath))
        {
            var safeName = SanitizeFileName(Path.GetFileNameWithoutExtension(newName), "Nota");
            var fileName = GetUniqueFileName(parentDirectory, safeName, Path.GetExtension(fullPath));
            targetPath = Path.Combine(parentDirectory, fileName);
            File.Move(fullPath, targetPath);
        }
        else
        {
            var safeName = SanitizeFileName(newName, "Pasta");
            var directoryName = GetUniqueDirectoryName(parentDirectory, safeName);
            targetPath = Path.Combine(parentDirectory, directoryName);
            Directory.Move(fullPath, targetPath);
        }

        Reload();
        return NormalizePath(Path.GetRelativePath(_rootPath, targetPath));
    }

    public string MoveItemToTrash(string itemPath)
    {
        if (string.IsNullOrWhiteSpace(itemPath))
        {
            throw new InvalidOperationException("Repository root cannot be deleted.");
        }

        var fullPath = GetSafeFullPath(itemPath);
        if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
        {
            throw new InvalidOperationException("Selected item does not exist.");
        }

        if (IsTrashPath(fullPath))
        {
            throw new InvalidOperationException("Trash items cannot be deleted again from the main tree.");
        }

        var trashDirectory = Path.Combine(_rootPath, TrashDirectoryName);
        Directory.CreateDirectory(trashDirectory);

        var baseName = Path.GetFileNameWithoutExtension(fullPath);
        var extension = File.Exists(fullPath) ? Path.GetExtension(fullPath) : string.Empty;
        var trashedName = extension.Length == 0
            ? GetUniqueDirectoryName(trashDirectory, Path.GetFileName(fullPath))
            : GetUniqueFileName(trashDirectory, baseName, extension);
        var trashPath = Path.Combine(trashDirectory, trashedName);

        if (File.Exists(fullPath))
        {
            File.Move(fullPath, trashPath);
        }
        else
        {
            Directory.Move(fullPath, trashPath);
        }

        Reload();
        return NormalizePath(Path.GetRelativePath(_rootPath, trashPath));
    }

    private void Reload()
    {
        _notes.Clear();
        _tree.Clear();

        foreach (var filePath in Directory
            .EnumerateFiles(_rootPath, "*.md", SearchOption.AllDirectories)
            .Where(path => !IsTrashPath(path))
            .OrderBy(path => path))
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
        var fileContent = File.ReadAllText(filePath);
        var frontmatter = ParseFrontmatter(fileContent);
        var relativePath = Path.GetRelativePath(_rootPath, filePath);
        var normalizedPath = NormalizePath(relativePath);
        var createdAt = frontmatter.Created ?? File.GetCreationTime(filePath);
        var updatedAt = frontmatter.Updated ?? File.GetLastWriteTime(filePath);

        return new NoteItem
        {
            Id = normalizedPath,
            Title = GetTitle(frontmatter, filePath),
            Markdown = frontmatter.Body,
            Path = normalizedPath,
            Type = frontmatter.Type,
            Status = frontmatter.Status,
            Tags = frontmatter.Tags,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    private IReadOnlyList<RepositoryNode> BuildTree(string directoryPath)
    {
        var children = new List<RepositoryNode>();

        foreach (var childDirectory in Directory.EnumerateDirectories(directoryPath).OrderBy(path => path))
        {
            if (IsTrashPath(childDirectory))
            {
                continue;
            }

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

    private bool IsTrashPath(string fullPath)
    {
        var normalizedPath = Path.GetFullPath(fullPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var trashPath = Path.GetFullPath(Path.Combine(_rootPath, TrashDirectoryName));

        return normalizedPath.Equals(trashPath, StringComparison.OrdinalIgnoreCase)
            || normalizedPath.StartsWith(trashPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
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

    private static string GetTitle(ParsedFrontmatter frontmatter, string filePath)
    {
        if (!string.IsNullOrWhiteSpace(frontmatter.Title))
        {
            return frontmatter.Title;
        }

        var heading = frontmatter.Body
            .Split(["\r\n", "\n"], StringSplitOptions.None)
            .Select(line => line.Trim())
            .FirstOrDefault(line => line.StartsWith("# ", StringComparison.Ordinal));

        return string.IsNullOrWhiteSpace(heading)
            ? Path.GetFileNameWithoutExtension(filePath)
            : heading[2..].Trim();
    }

    private static ParsedFrontmatter ParseFrontmatter(string content)
    {
        var normalizedContent = content.ReplaceLineEndings("\n");
        if (!normalizedContent.StartsWith("---\n", StringComparison.Ordinal))
        {
            return new ParsedFrontmatter { Body = content };
        }

        var endIndex = normalizedContent.IndexOf("\n---\n", 4, StringComparison.Ordinal);
        if (endIndex < 0)
        {
            return new ParsedFrontmatter { Body = content };
        }

        var frontmatterText = normalizedContent[4..endIndex];
        var body = normalizedContent[(endIndex + 5)..];
        var parsed = new ParsedFrontmatter { Body = body };

        foreach (var rawLine in frontmatterText.Split('\n'))
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var separatorIndex = line.IndexOf(':', StringComparison.Ordinal);
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = TrimYamlValue(line[(separatorIndex + 1)..].Trim());

            switch (key.ToLowerInvariant())
            {
                case "title":
                    parsed.Title = value;
                    break;
                case "type":
                    parsed.Type = string.IsNullOrWhiteSpace(value) ? "note" : value;
                    break;
                case "tags":
                    parsed.Tags = ParseTags(value);
                    break;
                case "status":
                    parsed.Status = string.IsNullOrWhiteSpace(value) ? "draft" : value;
                    break;
                case "created":
                    parsed.Created = ParseDate(value);
                    break;
                case "updated":
                    parsed.Updated = ParseDate(value);
                    break;
            }
        }

        return parsed;
    }

    private static string ComposeMarkdownFile(NoteItem note)
    {
        var created = FormatDate(note.CreatedAt);
        var updated = FormatDate(note.UpdatedAt);
        var tags = note.Tags.Count == 0
            ? "[]"
            : $"[{string.Join(", ", note.Tags.Select(EscapeYamlListValue))}]";
        var body = note.Markdown.ReplaceLineEndings(Environment.NewLine);

        return string.Join(Environment.NewLine, [
            "---",
            $"title: {EscapeYamlValue(note.Title)}",
            $"type: {EscapeYamlValue(note.Type)}",
            $"tags: {tags}",
            $"status: {EscapeYamlValue(note.Status)}",
            $"created: {created}",
            $"updated: {updated}",
            "---",
            body
        ]);
    }

    private static IReadOnlyList<string> ParseTags(string value)
    {
        var trimmedValue = value.Trim();
        if (trimmedValue is "[]" or "")
        {
            return [];
        }

        if (trimmedValue.StartsWith('[') && trimmedValue.EndsWith(']'))
        {
            trimmedValue = trimmedValue[1..^1];
        }

        return trimmedValue
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(TrimYamlValue)
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .ToArray();
    }

    private static DateTime? ParseDate(string value) =>
        DateTime.TryParse(value, null, System.Globalization.DateTimeStyles.RoundtripKind, out var date)
            ? date
            : null;

    private static string FormatDate(DateTime value) =>
        value.ToUniversalTime().ToString("O");

    private static string TrimYamlValue(string value)
    {
        var trimmedValue = value.Trim();
        return trimmedValue.Length >= 2
            && ((trimmedValue.StartsWith('"') && trimmedValue.EndsWith('"'))
                || (trimmedValue.StartsWith('\'') && trimmedValue.EndsWith('\'')))
            ? trimmedValue[1..^1]
            : trimmedValue;
    }

    private static string EscapeYamlValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "\"\"";
        }

        return value.Any(character => character is ':' or '#' or '[' or ']' or ',' or '"' or '\'')
            ? $"\"{value.Replace("\"", "\\\"")}\""
            : value;
    }

    private static string EscapeYamlListValue(string value) =>
        EscapeYamlValue(value);

    private sealed class ParsedFrontmatter
    {
        public string? Title { get; set; }

        public string Type { get; set; } = "note";

        public IReadOnlyList<string> Tags { get; set; } = [];

        public string Status { get; set; } = "draft";

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public required string Body { get; init; }
    }
}

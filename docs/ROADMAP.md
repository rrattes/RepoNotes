# RepoNotes Roadmap

## Current State

RepoNotes has a buildable Avalonia UI MVP with mocked local repository data, MVVM view models, a dark productivity-oriented interface, and a basic test project.

## Near Term

- Keep documentation and task logs updated in the repository.
- Improve Markdown editing ergonomics without adding complex persistence.
- Replace static visual preview with a simple Markdown rendering path when requested.
- Add focused UI tests or view model tests for selection/editing behavior.

## Next Product Milestones

1. File-backed notes
   - Read/write Markdown files from a chosen local repository.
   - Preserve folder structure.
   - Keep changes explicit and recoverable.

2. Repository navigation
   - Load real folders and Markdown files.
   - Add refresh and basic file operations.
   - Keep mock data available for design/dev mode if useful.

3. Search
   - Local text search over Markdown notes.
   - Fast filtering in the sidebar and search box.

4. Markdown preview
   - Render headings, lists, links, code, blockquotes, and tables.
   - Keep preview visually consistent with the dark UI guide.

## Guardrails

- Do not replace Avalonia UI.
- Preserve MVVM.
- Avoid login, cloud, sync, AI, or database work unless specifically requested.
- Prefer small commits with clear validation.

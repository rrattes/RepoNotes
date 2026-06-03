# RepoNotes Roadmap

## Current State

RepoNotes has a buildable Avalonia UI MVP with MVVM view models, a dark productivity-oriented interface, a basic test project, and initial local Markdown repository loading/saving.

## Near Term

- Keep documentation and task logs updated in the repository.
- Improve Markdown editing ergonomics without adding complex persistence.
- Replace static visual preview with a simple Markdown rendering path when requested.
- Add focused UI tests or view model tests for selection/editing behavior.
- Continue visual polish carefully; custom integrated window chrome is started/completed with real window controls.
- Continue evolving document navigation; functional note tabs are started/completed for opening, switching, saving, and closing notes.
- Continue the Markdown Power Editor path before WYSIWYG work; Split View is started/completed as the first step.
- Improve workspace layout for smaller screens; collapsible left/right side panels are started/completed.
- Plan Visual Markdown Mode / WYSIWYG-lite only after rich preview, toolbar, tabs, and editor stability are validated.

## Next Product Milestones

1. File-backed notes
   - Basic read/write Markdown files from a local sample repository is started.
   - YAML frontmatter support is started for title, type, tags, status, created, and updated metadata.
   - Initial compact UI for editing frontmatter metadata is started for `type`, `tags`, and `status`.
   - Save feedback now distinguishes saved, changed, saving, and save error states.
   - Functional note tabs now preserve unsaved edits per tab. Switching notes/tabs does not auto-save; closing a dirty tab, renaming, or deleting saves first and blocks the action if saving fails.
   - Preserve folder structure in the loaded tree.
   - Keep changes explicit and recoverable.

2. Repository navigation
   - Load real folders and Markdown files from the configured local repository.
   - Let the user open or switch to a local repository folder and persist the last opened repository in local settings.
   - Basic file operations started with automatic creation, rename, and trash-based delete for Markdown notes and folders.
   - Simple name prompts are started for creating and renaming notes/folders.
   - Trash restore, permanent delete, and empty-trash flows are started with `.reponotes-trash` kept out of the main tree/search.
   - Initial wiki-style internal links are started with `[[Nome da Nota]]`, title/file-name resolution, broken-link detection, and click-to-open from the preview/info panel.
   - Opening a note from tree navigation or internal links opens a real tab or activates the existing tab without creating duplicates.
   - Add refresh and confirmation UX for destructive trash actions.
   - Keep mock data available for design/dev mode if useful.

3. Technical note templates
   - Initial code-backed templates are started for free notes, runbooks, technical handovers, incidents, scripts, prompts, meetings, checklists, applications, and servers.
   - New note creation now uses the free-note template internally while preserving the existing simple UI.
   - A compact sidebar UI for choosing templates is started; `Nova nota` remains the fast free-note action and `Novo por template` creates from the selected template.
   - Future rounds may improve template selection ergonomics without adding custom template editing yet.
   - Keep templates local, simple, and Markdown/frontmatter based; do not add marketplace, plugins, cloud, or sync.

4. Lightweight Technical Entities
   - Add an incremental local entity layer for Application, Server, Network Device, Site, Environment, IP / Endpoint, Owner / Team, and Vendor / Product.
   - Relate notes, runbooks, scripts, handovers, incidents, and project docs to one or more entities.
   - Add future navigation and export grouped by application, site, owner, or environment.
   - Keep scope lightweight; do not turn RepoNotes into a full NetBox replacement.
   - Consider future NetBox import/integration after MVP, not during MVP.

5. User-Managed Encryption
   - Add optional password-based encryption controlled by the user, independent of Windows login, PC password, domain account, or administrator permissions.
   - Support future encryption scopes for folder, subfolder, or entire repository.
   - Define storage format before UI implementation.
   - Define unlock behavior and locked-state handling before advanced UI.
   - Define how search, preview, indexing, and export behave for locked content.
   - Define UX for lost password, making clear that encrypted content is unrecoverable if the password is lost.
   - Evaluate backup and portability impact so encrypted content can remain local-first and repository-portable when possible.
   - Do not use DPAPI or Windows Credential Manager as the primary model for encrypted notes/repositories.

6. Search
   - Local in-memory text search over Markdown notes by title, file name, path, and content is started.
   - Sidebar tag filtering from real frontmatter tags is started, including tag counts, active filter state, clear action, and combination with text search.
   - Search feedback now includes debounce, result count, empty state, clear action, `Ctrl+K` focus, and subtle matched-note highlighting.
   - Fast filtering in the sidebar and search box.
   - Future encrypted/locked content must not be indexed or searched while locked.

7. Markdown Power Editor
   - Render headings, paragraphs, lists, simple checklists, links, code blocks, blockquotes, and simple pipe tables from the current note Markdown.
   - Rich inline preview is started/completed for bold, italic, bold+italic, inline code, and visual links using native Avalonia inline runs.
   - Preview markers are cleaned up for headings, quotes, code fences, bullets, and checklists so the preview reads as rendered Markdown while the editor remains plain Markdown.
   - Central Editor/Preview/Split mode toggle is started/completed so users can switch the main workspace between raw Markdown editing, rendered Markdown preview, and side-by-side editing/preview.
   - Split View shows the Markdown source editor on the left and the native visual preview on the right, using the same `MarkdownPreviewService` output as Preview mode.
   - Split View now includes a resizable divider between editor and preview.
   - Workspace side panels are collapsible so the central editor/preview area can gain space on smaller screens.
   - Markdown formatting keyboard shortcuts are started/completed for bold, italic, headings, lists, checklist, code, quote, and contextual link/search behavior.
   - A compact Command Palette is started/completed for editor modes, Markdown formatting, basic insertions, save, and safe note/folder actions.
   - Future Split View improvements may include scroll synchronization between editor and preview.
   - Keep preview visually consistent with the dark UI guide.

8. Visual Markdown Mode / WYSIWYG-lite
   - Future milestone after the Markdown Power Editor path, including rich preview, toolbar Markdown, functional tabs, Split View, and editor save-state stability.
   - Markdown remains the source of truth saved to local `.md` files.
   - Markdown Mode must remain available; Visual Mode must not replace manual Markdown editing.
   - Evaluate WebView2/editor-web as the preferred path for real WYSIWYG quality through an isolated technical spike before adding packages or runtime dependencies.
   - If WebView2 proves too heavy for packaging or architecture, continue with native Avalonia or hybrid WYSIWYG-lite improvements.
   - Initial scope should cover H1/H2/H3, bold, italic, bold italic, lists, checklist, quote, inline code, code block, and visual links.
   - Frontmatter remains managed by parsing/storage and the Info panel, not edited as rich visual content.

## Guardrails

- Do not replace Avalonia UI.
- Preserve MVVM.
- Avoid login, cloud, sync, AI, or database work unless specifically requested.
- Prefer small commits with clear validation.

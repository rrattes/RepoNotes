# RepoNotes Roadmap

## Current State

RepoNotes has a buildable Avalonia UI MVP with MVVM view models, a dark productivity-oriented interface, a basic test project, and initial local Markdown repository loading/saving.

## Near Term

- Keep documentation and task logs updated in the repository.
- Improve Markdown editing ergonomics without adding complex persistence.
- Replace static visual preview with a simple Markdown rendering path when requested.
- Add focused UI tests or view model tests for selection/editing behavior.

## Next Product Milestones

1. File-backed notes
   - Basic read/write Markdown files from a local sample repository is started.
   - YAML frontmatter support is started for title, type, tags, status, created, and updated metadata.
   - Initial compact UI for editing frontmatter metadata is started for `type`, `tags`, and `status`.
   - Save feedback now distinguishes saved, changed, saving, and save error states.
   - Switching notes now auto-saves pending changes and blocks the switch if saving fails.
   - Preserve folder structure in the loaded tree.
   - Keep changes explicit and recoverable.

2. Repository navigation
   - Load real folders and Markdown files from the configured local repository.
   - Let the user open or switch to a local repository folder and persist the last opened repository in local settings.
   - Basic file operations started with automatic creation, rename, and trash-based delete for Markdown notes and folders.
   - Add refresh, restore from trash, and permanent delete flows.
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
   - Fast filtering in the sidebar and search box.
   - Future encrypted/locked content must not be indexed or searched while locked.

7. Markdown preview
   - Render headings, paragraphs, lists, simple checklists, links, code blocks, blockquotes, and simple pipe tables from the current note Markdown.
   - Keep preview visually consistent with the dark UI guide.

## Guardrails

- Do not replace Avalonia UI.
- Preserve MVVM.
- Avoid login, cloud, sync, AI, or database work unless specifically requested.
- Prefer small commits with clear validation.

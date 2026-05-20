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
   - Save feedback now distinguishes saved, changed, saving, and save error states.
   - Switching notes now auto-saves pending changes and blocks the switch if saving fails.
   - Preserve folder structure in the loaded tree.
   - Keep changes explicit and recoverable.

2. Repository navigation
   - Load real folders and Markdown files from the configured local repository.
   - Add refresh and basic file operations.
   - Keep mock data available for design/dev mode if useful.

3. Lightweight Technical Entities
   - Add an incremental local entity layer for Application, Server, Network Device, Site, Environment, IP / Endpoint, Owner / Team, and Vendor / Product.
   - Relate notes, runbooks, scripts, handovers, incidents, and project docs to one or more entities.
   - Add future navigation and export grouped by application, site, owner, or environment.
   - Keep scope lightweight; do not turn RepoNotes into a full NetBox replacement.
   - Consider future NetBox import/integration after MVP, not during MVP.

4. User-Managed Encryption
   - Add optional password-based encryption controlled by the user, independent of Windows login, PC password, domain account, or administrator permissions.
   - Support future encryption scopes for folder, subfolder, or entire repository.
   - Define storage format before UI implementation.
   - Define unlock behavior and locked-state handling before advanced UI.
   - Define how search, preview, indexing, and export behave for locked content.
   - Define UX for lost password, making clear that encrypted content is unrecoverable if the password is lost.
   - Evaluate backup and portability impact so encrypted content can remain local-first and repository-portable when possible.
   - Do not use DPAPI or Windows Credential Manager as the primary model for encrypted notes/repositories.

5. Search
   - Local text search over Markdown notes.
   - Fast filtering in the sidebar and search box.

6. Markdown preview
   - Render headings, lists, links, code, blockquotes, and tables.
   - Keep preview visually consistent with the dark UI guide.

## Guardrails

- Do not replace Avalonia UI.
- Preserve MVVM.
- Avoid login, cloud, sync, AI, or database work unless specifically requested.
- Prefer small commits with clear validation.

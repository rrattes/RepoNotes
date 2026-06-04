# RepoNotes vNext Architecture Plan

## Purpose

This document records the planned architecture direction for RepoNotes vNext. It is a planning document only; no implementation is implied by this file.

RepoNotes vNext is now a full web product direction. The frontend remains React, Vite, and TypeScript. The product path requires a backend API, a database strategy, service abstractions, and a serious security model before any public or broader-network deployment.

Tauri is no longer the main architecture path. It remains a possible future packaging option or alternate desktop wrapper after the web architecture is proven.

## React/Vite Frontend

- Create and evolve the vNext app under `apps/reponotes-vnext/`.
- Use React, Vite, and TypeScript as the primary frontend stack.
- Start with mock data and no real persistence until contracts are explicit.
- Keep UI state, editor state, tabs, navigation, Info panel state, and autosave state explicit and testable.
- Do not promote the old visual spike directly into product code.
- The UI must call service abstractions rather than coupling components directly to a backend or database.

## Backend API Future

- vNext requires a backend web API for the real product path.
- The backend owns persistence, path validation, note CRUD, metadata writes, trash operations, authorization, and audit-ready events.
- Frontend components must not perform raw database or filesystem work.
- API shape should be designed around product operations: repository navigation, notes, folders, metadata, tags, trash, autosave, exports, health scoring, and protected-note state.
- `RepositoryService` and `StorageService` abstractions should sit between UI flows and backend/storage implementations so the frontend is not locked to one backend choice.

## Database Strategy

### SQLite MVP

- SQLite is acceptable for a personal/local MVP.
- It keeps local/private usage simple and can support early CRUD, metadata, tags, trash, autosave state, and basic audit-event capture.
- The MVP should still preserve exportable Markdown as the user-facing portable format.

### PostgreSQL Future

- PostgreSQL is the likely future database for server, team, or multi-user installations.
- Schema and service contracts should avoid SQLite-only assumptions where practical.
- Migration strategy should be planned before multi-user or hosted deployments.

## Security / Auth Boundary

- MVP must not be exposed on the public internet.
- A localhost or private-network MVP may start without complex authentication if private data is not publicly reachable.
- If exposed beyond localhost/private network, strong authentication is required.
- Authorization must be enforced server-side.
- TLS is mandatory for exposed deployments.
- Audit logging is mandatory for exposed deployments.
- Amateur access control is prohibited.
- Private operational documentation must not be exposed publicly without a serious security architecture.
- Security decisions must be made before server/multi-user deployment, not patched on afterward.

## Audit Log Future

- Audit logging is not required for the earliest local-only MVP, but the architecture should leave room for it.
- Future audit events should cover login/session events, note reads/writes, metadata changes, exports, trash operations, protected-note unlock attempts, and administrative changes.
- Audit logs must be server-side for exposed deployments.

## Ghost Is Not Core Backend

- Ghost must not be used as RepoNotes' primary backend, database, or source of truth.
- Ghost may be considered only as a future publication/export destination.
- RepoNotes owns its documentation workflow, metadata, health scoring, protected notes, and operational context.

## Tauri Is No Longer Main Path

- Do not plan Tauri as a primary phase.
- Do not couple architecture to Tauri commands.
- Tauri may be revisited later as an optional wrapper if the web product needs desktop packaging.
- Browser use, corporate-computer access, and future mobile access are now stronger drivers than desktop packaging.

## Markdown As Primary Format

- Markdown remains the primary saved and exportable format.
- Notes should stay readable outside RepoNotes where practical.
- Frontmatter-like metadata stores structured fields, but the body remains Markdown.
- Export flows should prefer clean Markdown and Confluence-ready Markdown/HTML rather than proprietary formats.
- Database storage must not erase the ability to export clean Markdown.

## Visual Markdown Editor Strategy

- vNext is Visual Markdown Editor-first.
- The user should be able to type Markdown syntax and see visual formatting while editing.
- The editor must preserve clean Markdown round-trip quality.
- A separate Preview mode is not part of the initial vNext MVP.
- Split mode is not the main MVP editing model.
- The editor library or custom strategy must support headings, bold, italic, bold italic, lists, checklist, quote, inline code, code block, links, and basic tables.
- Frontmatter should not become rich visual document content; it belongs in parsing/storage and the Info panel.
- Any editor that produces messy Markdown should be rejected.

### Visual Editor Spike: Milkdown/Crepe

An initial vNext spike uses `@milkdown/crepe` inside the React/Vite shell to evaluate a visual Markdown-first editor without adding backend persistence.

Early findings:

- Crepe integrates cleanly in the React shell as an isolated component mounted into a DOM root.
- The spike can load Markdown from mock data and expose generated Markdown back to React state through Milkdown's markdown update listener.
- Visual editing is promising for headings, blockquotes, lists/checklists, tables, links, inline formatting, and fenced code blocks.
- The spike keeps Markdown in memory only; it does not prove autosave, backend persistence, frontmatter preservation, protected notes, or multi-note synchronization.
- Production builds pass, but Crepe pulls in a large JavaScript chunk due to editor/code features. Bundle size and code splitting must be evaluated before adopting it as the final editor stack.

Next evaluation criteria:

- Round-trip frontmatter without exposing YAML as rich body content.
- Paste/edit behavior for operational Markdown documents.
- Generated Markdown cleanliness for tables, checklists, links, and code blocks.
- Bundle size and startup impact in a browser-first app.
- Theming consistency with the approved RepoNotes dark UI.

## Autosave Strategy

- vNext uses autosave with debounce instead of a primary Save button.
- Autosave status should be visible but quiet: saved, saving, changed, and error states.
- Autosave must operate per active note/tab.
- Autosave writes through the backend API and storage service.
- Autosave must not corrupt Markdown if the editor is mid-composition.
- Save errors must preserve the user's current editor content.
- Password protected notes require special autosave rules because locked content cannot be edited or exported while locked.

## Storage / Repository Strategy

- `RepositoryService` represents repository-level operations such as listing notes/folders, opening notes, navigating trash, and querying metadata.
- `StorageService` represents persistence operations such as saving note body, metadata, protected payloads, trash moves, export generation, and backup/restore.
- The frontend should depend on these contracts, not concrete SQLite/PostgreSQL/API details.
- `.reponotes` and `.reponotes-trash` concepts may remain as export/import or repository-compatibility concepts, but the web backend owns the actual storage implementation.

## Trash Strategy

- Delete should move notes/folders to trash first.
- Restore should recover items when possible and avoid overwriting conflicts.
- Permanent delete and empty trash require explicit confirmation UX.
- Trash operations must validate targets and never operate outside the intended repository/storage boundary.
- Protected notes in trash remain protected; deleting/restoring must not decrypt them.

## Metadata / Frontmatter Strategy

- Metadata stores structured fields such as title, type, tags, status, created, updated, owner, review dates, application, environment, IBX/site, criticality, and related entities.
- Tags are edited and shown in the Info panel.
- The Info panel is closed by default.
- Metadata changes use the same autosave pipeline as note body changes where safe.
- Metadata must support Documentation Health Score, Review Cycle, Application Documentation Pack, RACI, Runbooks, Handover Packs, and export.
- Export should be able to reconstruct clean Markdown with frontmatter when needed.

## Password Protected Notes Strategy

Password Protected Notes are a future feature and must not be implemented before a technical design phase.

Initial scope:

- Protect individual notes first.
- Prefer client-side / zero-knowledge encryption where possible.
- Use a password created by the user specifically for that note/content.
- The password is independent of Windows login, PC password, domain account, administrator permissions, DPAPI, and Windows Credential Manager.
- If the password is lost, the protected content is unrecoverable.

Required behavior:

- Locked protected content does not enter search, indexing, snippets, preview, Documentation Health Score content checks, or export.
- Export requires the note to be unlocked.
- Internal link resolution must not expose protected content while locked.
- Autosave must respect locked/unlocked state.
- Trash operations must move/delete encrypted payloads without decrypting them.
- The app must avoid storing passwords in clear text.
- The app must avoid writing plaintext protected content to persistent caches.
- Unlock state should be memory-only unless a later design explicitly permits a safer scoped cache.

Design questions before implementation:

- Encryption format and file/database structure.
- Whether encryption happens entirely client-side or with a server-assisted zero-knowledge model.
- How frontmatter/metadata identifies protected notes without exposing sensitive content.
- Whether title/tags/status remain visible when locked.
- How autosave works after unlock and before relock.
- How search, export, trash, broken links, orphan detection, and health scoring behave for locked notes.
- How backup/restore handles encrypted payloads.
- How to communicate irreversible password loss clearly.

## Architecture Risks

- Migrating all Avalonia features at once would create a large, fragile rewrite.
- Promoting the visual spike directly could carry mock assumptions into product code.
- A poor Visual Markdown editor choice could generate bad Markdown and undermine portability.
- Full web direction requires backend and security work that cannot be hand-waved.
- Amateur access control could expose private operational data and is prohibited.
- Password protected notes complicate autosave, search, export, internal links, trash, health scoring, and backup.
- Client-side / zero-knowledge encryption can reduce search/export/health-score capabilities while content is locked.
- Mobile support will require its own layout strategy rather than squeezing the desktop workspace into a phone viewport.
- Scope can grow too much unless vNext follows the roadmap phases.

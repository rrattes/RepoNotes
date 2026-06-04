# RepoNotes Roadmap

## Current State

RepoNotes has a functional Avalonia MVP with local Markdown files, tabs, search, tags, frontmatter, trash flows, preview, Markdown editing helpers, and tests. This version remains the functional reference and legacy baseline.

RepoNotes vNext is now a full web product direction built with React, Vite, and TypeScript. The current app under `apps/reponotes-vnext/` is a clean web shell with mock data and a Visual Markdown Editor spike. Tauri is no longer the main path; it remains a possible future packaging option after the web product architecture is proven.

## vNext Product Direction

- Full web-first technical documentation workspace.
- Visual Markdown Editor-first.
- Markdown remains the primary clean saved/exportable format.
- Users can type Markdown and see visual formatting while editing.
- No separate Preview mode in the initial vNext MVP.
- No Split mode as the primary MVP flow.
- Autosave with debounce.
- No primary Save button.
- Note name appears in tabs for navigation and as the first editable block inside the document.
- Tags and metadata live in the Info panel.
- Info panel is closed by default.
- Left sidebar is for navigation only.
- Left rail uses icons for global/workspace areas.
- Dark premium UI based on the approved reference image.
- Every visible button must perform a real action or be removed.
- Backend API and database become part of the product path.
- SQLite is acceptable for a personal/local MVP.
- PostgreSQL is the likely future path for server or multi-user installs.
- MVP must not be exposed on the public internet.
- If exposed beyond localhost/private network, strong auth, server-side authorization, TLS, and audit logging are mandatory.

## vNext Phases

### Phase 1 - Full Web Architecture Direction

- Record the full web-first product direction.
- Keep frontend stack as React/Vite/TypeScript.
- Move Tauri out of the main path and into future/alternative packaging.
- Define frontend/backend/storage boundaries early.
- Introduce `RepositoryService` and `StorageService` abstractions before real persistence work.
- Confirm Ghost is not the core backend and may only be a future publication/export target.

### Phase 2 - Visual Markdown Editor Validation

- Continue validating the Visual Markdown Editor-first experience.
- Confirm the editor can support operational documentation workflows without exposing raw debug UI.
- Validate headings, bold, italic, lists, checklist, quote, code, links, tables, and larger technical notes.
- Reject approaches that generate messy Markdown or make manual Markdown editing unreliable.
- Backlog: replace the current visual gutter with a real synchronized gutter based on ProseMirror blocks/lines if the editor architecture supports it cleanly.
- Status: started with an isolated Milkdown/Crepe spike in `apps/reponotes-vnext/`.

### Phase 3 - Markdown Round-trip And Frontmatter Boundary

- Preserve clean Markdown as source/export format.
- Keep frontmatter out of the rich editable document body.
- Validate generated Markdown cleanliness for tables, checklists, links, and code blocks.
- Define how metadata changes in the Info panel merge with editor body changes.

### Phase 4 - Web Backend API MVP

- Create a backend API for repositories, notes, metadata, trash, and autosave.
- Keep UI components away from raw backend/database details.
- Ensure all write paths go through server-side validation.
- Keep deployment local/private during MVP.

### Phase 5 - Local Database MVP

- Use SQLite for the personal/local MVP.
- Define schema for notes, folders/repository structure, metadata, tags, trash, and audit-ready events.
- Keep data exportable as Markdown.
- Plan PostgreSQL compatibility for future server/multi-user deployments.

### Phase 6 - Security/Auth Model

- Define security boundaries before any network exposure.
- Localhost/private-network MVP may start without complex auth.
- Public or wider network exposure requires strong authentication, server-side authorization, TLS, and audit logging.
- Amateur access control is prohibited.
- Private data must not be exposed publicly without serious security architecture.

### Phase 7 - Notes CRUD

- Create, read, update, rename, move to trash, restore, and permanently delete notes/folders through the backend API.
- Keep trash out of navigation/search/export/health scoring.
- Preserve tab behavior and editor state across CRUD operations.

### Phase 8 - Autosave

- Implement autosave with debounce.
- Track saved, saving, changed, and error states per note/tab.
- Avoid corrupting Markdown during editor composition.
- Preserve unsaved editor content if backend writes fail.

### Phase 9 - Metadata, Tags And Info Panel

- Read/write frontmatter-like metadata through the Info panel.
- Support title, type, status, tags, created, updated, owner, review date, application, environment, IBX/site, criticality, and related entities.
- Keep tags in the Info panel, not as a primary sidebar feature.

### Phase 10 - Application Documentation Pack

- Generate folder-pack documentation for applications by IBX/environment.
- Include overview, technical details, architecture, operations, access/security, monitoring, backup/DR, dependencies, runbooks, incidents, RACI, and change history.
- Ensure generated content is practical and exportable.

### Phase 11 - Documentation Health Score

- Score documentation completeness and freshness.
- Detect missing required sections, missing metadata, missing owner, missing RACI, missing review date, stale content, broken links, and orphan documentation.
- Keep scoring transparent and actionable.

### Phase 12 - Review Cycle / Expiration

- Add review/expiration fields and workflows.
- Surface notes and application packs needing review.
- Make overdue documentation visible without requiring cloud reminders.

### Phase 13 - RACI Builder

- Provide structured RACI creation and editing.
- Support application, environment, runbook, and process ownership views.
- Store output in Markdown/frontmatter-friendly format.

### Phase 14 - Runbook Builder

- Provide structured runbook sections: purpose, prerequisites, steps, rollback, validation, escalation, owner, and review cycle.
- Keep runbooks Markdown-first and exportable.

### Phase 15 - Handover Pack

- Generate handover bundles for projects, systems, applications, incidents, and operational transitions.
- Include ownership, current state, risks, access/security notes, open actions, dependencies, and runbooks.

### Phase 16 - Broken Links / Orphan Docs

- Detect broken wiki-style links and file references.
- Detect orphan notes not connected to folders, applications, owners, tags, or entities.
- Feed link/orphan status into Documentation Health Score.

### Phase 17 - Lightweight Technical Entities

- Add a lightweight entity layer for applications, servers, network devices, sites, environments, endpoints, owner teams, vendors, and products.
- Enrich documentation context without replacing NetBox/DCIM/IPAM.
- Keep entity data portable through export and clear storage contracts.

### Phase 18 - Confluence-ready Export

- Export clean Markdown and Confluence-ready copy/paste content.
- Include useful metadata when appropriate.
- Do not implement Atlassian login, OAuth, or direct publishing in this phase.
- Ghost may be considered only as a future publication/export destination, not as the core backend.

### Phase 19 - Password Protected Notes Design

- Design password protected notes before implementation.
- Prioritize client-side / zero-knowledge encryption when possible.
- Define encryption format, unlock flow, autosave behavior, search exclusion, export requirements, links behavior, trash behavior, health-score behavior, and backup behavior.
- Password must be independent of Windows, domain account, administrator permissions, DPAPI, and Windows Credential Manager.
- If the password is lost, protected content is unrecoverable.

Password Protected Notes must not be implemented before this design phase because it affects search, autosave, export, internal links, trash, metadata, Documentation Health Score, deployment, and backup.

### Phase 20 - Deployment / Backup

- Define supported personal/local deployment.
- Define backup and restore strategy for SQLite/local data.
- Plan PostgreSQL deployment for future server/multi-user setups.
- Require auth/TLS/audit log before any public or broader network exposure.
- Keep Tauri as a possible future desktop wrapper, not the main roadmap path.

## Guardrails

- Do not migrate all features at once.
- Do not promote the old spike directly into product code.
- Keep vNext clean under `apps/reponotes-vnext/`.
- Keep Markdown as the primary format.
- Do not expose private data publicly in the MVP.
- Do not implement amateur access control.
- Do not use Ghost as the main backend.
- Prefer small, verifiable phases and commits.

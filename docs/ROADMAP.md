# RepoNotes Roadmap

## Current State

RepoNotes has a functional Avalonia MVP with local Markdown files, tabs, search, tags, frontmatter, trash flows, preview, Markdown editing helpers, and tests. This version remains the functional reference and legacy baseline.

RepoNotes vNext will be rebuilt as a clean React/Vite/TypeScript application, with Tauri introduced after the frontend shell, editor strategy, and product direction are validated. The old visual spike in `spikes/tauri-react-visual/` is a reference artifact only, not the product base.

## vNext Product Direction

- Visual Markdown Editor-first.
- Markdown remains the primary clean saved/exportable format.
- Users can type Markdown and see visual formatting while editing.
- No separate Preview mode in the initial vNext MVP.
- No Split mode as the primary MVP flow.
- Autosave with debounce.
- No primary Save button.
- Note name appears primarily in the tab.
- Tags and metadata live in the Info panel.
- Info panel is closed by default.
- Left sidebar is for navigation only.
- Left rail uses icons for global/workspace areas.
- Dark premium UI based on the approved reference image.
- Every visible button must perform a real action or be removed.

## vNext Phases

### Phase 0 — Definitive UI and Product Direction

- Record the approved product direction and definitive UI rules.
- Confirm that RepoNotes is an operational documentation product, not a generic Obsidian competitor.
- Preserve Avalonia as a functional reference while planning a clean vNext implementation.

### Phase 1 — Clean React/Vite Shell

- Create `apps/reponotes-vnext/` as a clean React/Vite/TypeScript app.
- Rebuild the approved dark premium layout with mock data.
- Keep the shell free of real filesystem, storage, encryption, or Tauri code.
- Prioritize maximum writing area, compact tabs, left icon rail, navigation sidebar, and a closed-by-default Info panel.

### Phase 2 — Visual Markdown Editor Spike

- Evaluate editor libraries and implementation approaches for Visual Markdown editing.
- Confirm Markdown round-trip quality.
- Validate headings, bold, italic, lists, checklist, quote, code, links, and frontmatter boundaries.
- Reject approaches that generate messy Markdown or make manual Markdown editing unreliable.
- Status: started with an isolated Milkdown/Crepe spike in `apps/reponotes-vnext/`.
- Next validation: frontmatter boundaries, larger operational notes, Markdown cleanliness, and bundle/startup impact.

### Phase 3 — Tauri Desktop Shell

- Wrap the clean React/Vite shell in Tauri.
- Validate Windows window chrome, startup time, memory footprint, packaging, keyboard shortcuts, and local filesystem permission model.
- Keep Tauri commands behind a clean contract.

### Phase 4 — Local Filesystem MVP

- Add local repository open/select flow.
- Load folders and `.md` files from disk.
- Preserve Markdown as plain files.
- Add autosave with debounce and clear dirty/saved/error states.
- Keep operations local-first and predictable.

### Phase 5 — Core Notes Repository Features

- Create, rename, move to trash, restore, and permanently delete notes/folders.
- Keep `.reponotes` and `.reponotes-trash` out of navigation/search.
- Add tabs, keyboard shortcuts, command palette, and context menus only when actions are real.

### Phase 6 — Metadata, Tags and Info Panel

- Read/write frontmatter for title, type, status, tags, created, updated, owner, review dates, and technical context.
- Put tags in the Info panel, not as primary sidebar content.
- Keep Info panel closed by default.

### Phase 7 — Application Documentation Pack

- Generate folder-pack documentation for applications by IBX/environment.
- Include structured files for overview, technical details, architecture, operations, access/security, monitoring, backup/DR, dependencies, runbooks, incidents, RACI, and change history.
- Ensure generated content is practical and exportable.

### Phase 8 — Documentation Health Score

- Score documentation completeness and freshness.
- Detect missing required sections, missing metadata, missing owner, missing RACI, missing review date, and stale content.
- Keep scoring transparent and actionable.

### Phase 9 — Review Cycle

- Add review/expiration fields and workflows.
- Surface notes and application packs needing review.
- Make overdue documentation visible without adding cloud reminders.

### Phase 10 — RACI Builder

- Provide structured RACI creation and editing.
- Support application, environment, runbook, and process ownership views.
- Store output in Markdown/frontmatter-friendly format.

### Phase 11 — Runbook Builder

- Provide structured runbook sections: purpose, prerequisites, steps, rollback, validation, escalation, owner, and review cycle.
- Keep runbooks Markdown-first and exportable.

### Phase 12 — Handover Pack

- Generate handover bundles for projects, systems, applications, incidents, and operational transitions.
- Include ownership, current state, risks, access/security notes, open actions, dependencies, and runbooks.

### Phase 13 — Broken Links and Orphan Documentation

- Detect broken wiki-style links and file references.
- Detect orphan notes not connected to folders, applications, owners, tags, or entities.
- Feed link/orphan status into Documentation Health Score.

### Phase 14 — Lightweight Technical Entities

- Add a local lightweight entity layer for applications, servers, network devices, sites, environments, endpoints, owner teams, vendors, and products.
- Enrich documentation context without replacing NetBox/DCIM/IPAM.
- Keep entity data portable and local.

### Phase 15 — Confluence-ready Export

- Export clean Markdown and Confluence-ready copy/paste content.
- Include useful metadata when appropriate.
- Do not implement Atlassian login, OAuth, or direct publishing in this phase.

### Phase 16 — Password Protected Notes Design

- Design password protected notes before implementation.
- Define encryption format, unlock flow, autosave behavior, search exclusion, export requirements, links behavior, trash behavior, and health-score behavior.
- Password must be independent of Windows, domain account, administrator permissions, DPAPI, and Windows Credential Manager.
- If the password is lost, protected content is unrecoverable.

Password Protected Notes must not be implemented before this design phase because it affects search, autosave, export, internal links, trash, metadata, and Documentation Health Score.

### Phase 17 — Password Protected Notes Implementation

- Implement per-note password protection first.
- Keep protected content out of search/indexing while locked.
- Require unlock before visual editing and export.
- Avoid storing plaintext password or plaintext protected content in persistent caches.

### Phase 18 — Windows Packaging

- Build and validate Windows packaging.
- Test installation, updates, local filesystem permissions, startup time, and data portability.
- Confirm no cloud/login assumptions enter the product.

## Guardrails

- Do not migrate all features at once.
- Do not promote the old spike directly into product code.
- Keep vNext clean under `apps/reponotes-vnext/`.
- Keep Markdown as the primary format.
- Avoid login, cloud, sync, AI, or database work unless specifically requested.
- Prefer small, verifiable phases and commits.

# RepoNotes vNext Architecture Plan

## Purpose

This document records the planned architecture direction for RepoNotes vNext. It is a planning document only; no implementation is implied by this file.

RepoNotes vNext should be a clean React/Vite/TypeScript application first, then a Tauri desktop application after the frontend shell, Visual Markdown editor strategy, and local-first contracts are validated.

## React/Vite Frontend

- Create the vNext app under `apps/reponotes-vnext/`.
- Use React, Vite, and TypeScript as the primary frontend stack.
- Start with mock data and no filesystem access.
- Keep UI state, editor state, tabs, navigation, Info panel state, and autosave state explicit and testable.
- Do not promote the old visual spike directly into product code.

## Tauri Shell Future

- Add Tauri only after the React/Vite shell is stable.
- Use Tauri for desktop windowing, packaging, and controlled local filesystem access.
- Keep Tauri commands behind a narrow application service contract.
- Avoid mixing UI components with raw filesystem calls.
- Validate Windows startup time, memory usage, file permissions, window chrome, keyboard shortcuts, and packaging.

## Markdown As Primary Format

- Markdown remains the primary saved and exportable format.
- Notes should stay readable outside RepoNotes.
- Frontmatter stores structured metadata, but the body remains Markdown.
- Export flows should prefer clean Markdown and Confluence-ready Markdown/HTML rather than proprietary formats.

## Visual Markdown Editor Strategy

- vNext is Visual Markdown Editor-first.
- The user should be able to type Markdown syntax and see visual formatting while editing.
- The editor must preserve clean Markdown round-trip quality.
- A separate Preview mode is not part of the initial vNext MVP.
- Split mode is not the main MVP editing model.
- The editor library or custom strategy must support headings, bold, italic, bold italic, lists, checklist, quote, inline code, code block, and links.
- Frontmatter should not become rich visual document content; it belongs in parsing/storage and the Info panel.
- Any editor that produces messy Markdown should be rejected.

## Autosave Strategy

- vNext uses autosave with debounce instead of a primary Save button.
- Autosave status should be visible but quiet: saved, saving, changed, and error states.
- Autosave must operate per active note/tab.
- Autosave must not corrupt Markdown if the editor is mid-composition.
- Save errors must preserve the user's current editor content.
- Password protected notes require special autosave rules because locked content cannot be edited or exported while locked.

## Local Filesystem Strategy

- The repository is a local folder selected by the user.
- Markdown files are loaded from the local repository.
- File access should be mediated by a Tauri command/service layer.
- The frontend should not assume direct browser filesystem APIs for the desktop app.
- `.reponotes` stores local app metadata.
- `.reponotes-trash` stores trashed items and must stay out of navigation, search, export, and health scoring.

## Trash Strategy

- Delete should move notes/folders to `.reponotes-trash` first.
- Restore should recover items when possible and avoid overwriting conflicts.
- Permanent delete and empty trash require explicit confirmation UX.
- Trash operations must validate paths and never operate outside the repository trash directory.
- Protected notes in trash remain protected; deleting/restoring must not decrypt them.

## Metadata / Frontmatter Strategy

- Frontmatter stores structured fields such as title, type, tags, status, created, updated, owner, review dates, application, environment, IBX/site, criticality, and related entities.
- Tags are edited and shown in the Info panel.
- The Info panel is closed by default.
- Metadata changes use the same autosave pipeline as note body changes where safe.
- Metadata must support Documentation Health Score, Review Cycle, Application Documentation Pack, RACI, Runbooks, Handover Packs, and export.

## Password Protected Notes Strategy

Password Protected Notes are a future feature and must not be implemented before a technical design phase.

Initial scope:

- Protect individual notes first.
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

- Encryption format and file structure.
- How frontmatter identifies protected notes without exposing sensitive content.
- Whether title/tags/status remain visible when locked.
- How autosave works after unlock and before relock.
- How export, trash, broken links, orphan detection, and health scoring behave for locked notes.
- How to communicate irreversible password loss clearly.

## Architecture Risks

- Migrating all Avalonia features at once would create a large, fragile rewrite.
- Promoting the visual spike directly could carry mock assumptions into product code.
- A poor Visual Markdown editor choice could generate bad Markdown and undermine portability.
- Password protected notes complicate autosave, search, export, internal links, trash, and health scoring.
- Tauri filesystem access needs a clean contract to avoid UI/backend coupling.
- Scope can grow too much unless vNext follows the roadmap phases.

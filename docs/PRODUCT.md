# RepoNotes Product Definition

## Purpose

RepoNotes is a Windows desktop, local-first documentation application for technical operations. RepoNotes vNext will be rebuilt with React, Vite, and TypeScript, with Tauri planned after the frontend shell and editor strategy are validated.

The existing Avalonia application remains the functional reference and legacy MVP. It is not the base for vNext product UI work.

The goal is not to compete commercially as a generic note-taking app against Obsidian, Notion, or Evernote. RepoNotes should do well what generic note apps usually require plugins, manual conventions, or process discipline to do: structured technical documentation, operational governance, review cycles, application packs, RACI, handovers, runbooks, and export-ready documentation.

## Target User

- Developers, SREs, infrastructure teams, technical operators, and application owners maintaining operational documentation.
- Teams documenting applications, environments, runbooks, RACI, handovers, incidents, operational procedures, and governance material.
- Users who prefer local files, predictable desktop workflows, and exportable Markdown over cloud-first tools.

## Product Principles

- Local-first by default.
- Repository-oriented organization.
- Visual Markdown Editor-first in vNext, while keeping clean Markdown as the primary saved/exportable format.
- Markdown remains portable and editable outside RepoNotes.
- Lightweight technical entities enrich documentation with practical inventory context.
- Optional user-managed encryption protects local content with a password created by the user, independent of Windows or the PC account.
- Autosave with debounce replaces a primary Save button in the vNext MVP.
- Desktop productivity over marketing polish.
- Small, verifiable increments.
- No login, cloud, AI, sync, or database unless explicitly requested.

## vNext Product Direction

RepoNotes vNext is a local-first operational documentation editor, not a generic personal notes clone. It should focus on:

- Applications and their documentation packs.
- Environments and IBX/site context.
- Runbooks and operational procedures.
- RACI and ownership clarity.
- Handovers and transition packages.
- Documentation health, expiration, stale content, and broken links.
- Confluence-ready export for teams that still need to publish elsewhere.
- Password protected notes for sensitive local documentation.

The vNext MVP starts with a definitive dark premium UI, a clean React/Vite/TypeScript shell, and a Visual Markdown Editor-first workflow. The user should be able to type Markdown and see the visual effect while editing. A separate Preview mode and Split mode are not primary MVP concepts for vNext.

The approved visual direction prioritizes maximum writing area, compact tabs, a left navigation sidebar with icon rail, an Info panel closed by default, tags inside the Info panel, autosave status, and no non-functional buttons.

## Core Differentiators

1. **Application Documentation Pack**
   - Generate and maintain structured application documentation by environment/IBX, including overview, technical details, architecture, operations, access/security, monitoring, backup/DR, dependencies, runbooks, incidents, RACI, and change history.

2. **Documentation Health Score**
   - Score documents and application packs based on completeness, freshness, missing sections, broken links, stale ownership, missing RACI, missing runbooks, and unresolved metadata.

3. **Review Cycle / Expiration**
   - Track review dates, expiration dates, owners, and required refresh cycles for operational documentation.

4. **RACI Builder**
   - Provide a structured way to create and maintain RACI matrices for applications, environments, systems, and operational processes.

5. **Runbook Builder**
   - Help create repeatable operational runbooks with prerequisites, steps, rollback, validation, escalation, and ownership.

6. **Handover Pack**
   - Build handover-ready documentation bundles for projects, systems, incidents, operational transitions, and team changes.

7. **Broken Links and Orphan Documentation**
   - Detect broken internal links, orphan notes, unreferenced runbooks, and documentation that is not connected to applications, owners, or operational contexts.

8. **Lightweight Technical Entities**
   - Relate notes to lightweight applications, servers, sites, environments, endpoints, owner teams, vendors, and products without becoming a DCIM/IPAM source of truth.

9. **Confluence-ready Export**
   - Export clean Markdown or copy/paste-friendly content suitable for Confluence publishing without implementing Atlassian login or API publishing in the MVP.

10. **Password Protected Notes**
    - Protect individual sensitive notes with a user-managed password independent of Windows, with explicit locked-state behavior for search, export, autosave, links, trash, and health scoring.

## Lightweight Technical Entities

RepoNotes should support Lightweight Technical Entities: a small local layer of technical context inspired by inventory, DCIM, and IPAM workflows, without becoming a complete NetBox replacement.

Initial entity types:

- Application
- Server
- Network Device
- Site
- Environment
- IP / Endpoint
- Owner / Team
- Vendor / Product

These entities should help relate notes, runbooks, handovers, scripts, incidents, and project documentation to real technical contexts. A note should eventually be able to indicate which application, server, site, environment, owner, or related technical entity it belongs to.

This is a practical differentiator for RepoNotes: documentation stays local and Markdown-first, but can also be navigated and exported by technical context, such as application, site, owner, or environment.

NetBox-style data should enrich documentation, not replace a DCIM/IPAM source of truth. RepoNotes may store lightweight inventory context locally, but full rack management, cabling, patch panels, complete IPAM, VLAN lifecycle, and circuit management are out of scope. Future import or integration with NetBox may be considered after the MVP, but is not part of the MVP.

## User-Managed Encryption

RepoNotes should support optional User-Managed Encryption for local content. The encryption model is based on a password created by the user specifically for RepoNotes content, not on the Windows account password, PC login, corporate domain, administrator privileges, DPAPI, or Windows Credential Manager as the primary model.

The user should eventually be able to choose the encryption scope:

- A folder.
- A subfolder.
- An entire repository.

The password must be:

- Created by the user.
- Independent from the computer password.
- Independent from the Windows account.
- Independent from administrator permissions.
- Required to open or unlock encrypted content.

The user must not need administrator privileges to use this feature. If the password is lost, the encrypted content should be considered unrecoverable. Encrypted content should remain local-first and, when possible, portable with the repository.

Search, preview, indexing, and export must respect the locked/unlocked state of encrypted content. RepoNotes should avoid keeping plaintext copies of encrypted content in caches or indexes. Implementation should be incremental and should only start after the basic local notes workflow is stable.

## MVP Scope

- Avalonia UI desktop shell.
- MVVM structure.
- Mocked repository tree and notes.
- Three-column layout: sidebar, editor, preview/info panel.
- Product requirement for lightweight technical entities, to be implemented incrementally after core local-note workflows are stable.
- Product requirement for optional user-managed encryption, to be designed after core local-note workflows are stable.
- Dark premium productivity interface.
- Buildable .NET solution with tests.

## Out Of Scope For Now

- Authentication.
- Cloud storage.
- Synchronization.
- AI features.
- Database persistence.
- Marketplace/plugin architecture.
- Full DCIM/IPAM replacement features such as rack management, cabling, patch panels, VLAN lifecycle, complete IPAM, and circuit management.
- Windows-account-only encryption as the primary model for notes or repositories.
- Corporate key management, password recovery, password vaults, secure sharing between users, or cloud encryption.

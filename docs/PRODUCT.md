# RepoNotes Product Definition

## Purpose

RepoNotes is a Windows desktop, local-first notes application built with Avalonia UI. It is designed for technical documentation, projects, scripts, prompts, runbooks, handovers, and operational notes stored in local repositories.

The goal is not to compete commercially with Obsidian, Notion, or Evernote. RepoNotes should be a practical tool for real technical documentation workflows and personal/professional organization.

## Target User

- Developers and technical operators maintaining local project documentation.
- People who keep scripts, runbooks, prompts, and handover notes near their working repositories.
- Users who prefer local files and predictable desktop workflows over cloud-first tools.

## Product Principles

- Local-first by default.
- Repository-oriented organization.
- Markdown-first editing.
- Lightweight technical entities enrich documentation with practical inventory context.
- Optional user-managed encryption protects local content with a password created by the user, independent of Windows or the PC account.
- Desktop productivity over marketing polish.
- Small, verifiable increments.
- No login, cloud, AI, sync, or database unless explicitly requested.

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

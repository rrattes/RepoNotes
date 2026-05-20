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

## MVP Scope

- Avalonia UI desktop shell.
- MVVM structure.
- Mocked repository tree and notes.
- Three-column layout: sidebar, editor, preview/info panel.
- Product requirement for lightweight technical entities, to be implemented incrementally after core local-note workflows are stable.
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

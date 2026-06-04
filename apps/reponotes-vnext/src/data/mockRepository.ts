import type { MockNote, NoteTab, RailItemId, RepositoryNode } from "../types/reponotes";

export const visualMarkdownSpikeInitialMarkdown = `---
title: Application Documentation Pack
type: application-pack
status: draft
owner: Amer - Monitoring & Tools
tags:
  - application
  - la4
  - ops
---

# Application Documentation Pack

> Este pacote organiza a documentação técnica operacional de uma aplicação.

## Checklist

- [x] Owner definido
- [ ] RACI revisada

## Links

Veja [[10-RACI]] e [LibreNMS](https://www.librenms.org).

## Tabela

| Documento | Objetivo | Status |
|---|---|---|
| 00-Overview | Visão geral | Ready |
| 10-RACI | Responsabilidades | Missing |

\`\`\`powershell
dotnet test
\`\`\`
`;

export const railItems: Array<{ id: RailItemId; label: string; icon: string }> = [
  { id: "files", label: "Files", icon: "F" },
  { id: "search", label: "Search", icon: "/" },
  { id: "links", label: "Links", icon: "L" },
  { id: "tags", label: "Tags", icon: "#" },
  { id: "tasks", label: "Tasks", icon: "T" },
  { id: "entities", label: "Entities", icon: "E" },
  { id: "trash", label: "Trash", icon: "X" },
  { id: "settings", label: "Settings", icon: "S" }
];

export const repositoryTree: RepositoryNode[] = [
  { id: "inbox", name: "00 - Inbox", type: "folder" },
  {
    id: "applications",
    name: "01 - Applications",
    type: "folder",
    children: [
      {
        id: "ibx",
        name: "IBX",
        type: "folder",
        children: [
          {
            id: "la4",
            name: "LA4",
            type: "folder",
            children: [
              {
                id: "apps",
                name: "Applications",
                type: "folder",
                children: [
                  {
                    id: "librenms",
                    name: "LibreNMS",
                    type: "folder",
                    children: [
                      { id: "overview", name: "00-Overview.md", type: "note" },
                      { id: "technical", name: "01-Technical-Details.md", type: "note" },
                      { id: "monitoring", name: "05-Monitoring.md", type: "note" },
                      { id: "raci", name: "10-RACI.md", type: "note" }
                    ]
                  }
                ]
              }
            ]
          }
        ]
      }
    ]
  },
  {
    id: "runbooks",
    name: "Runbooks",
    type: "folder",
    children: [
      { id: "restart-service", name: "Restart Service.md", type: "note" },
      { id: "deploy-agent", name: "Deploy Local Agent.md", type: "note" }
    ]
  },
  {
    id: "governance",
    name: "Governance",
    type: "folder",
    children: [{ id: "monthly-review", name: "Monthly Review.md", type: "note" }]
  }
];

export const noteTabs: NoteTab[] = [
  {
    id: "overview",
    title: "00-Overview.md",
    path: "01 - Applications/IBX/LA4/Applications/LibreNMS/00-Overview.md"
  },
  {
    id: "pack",
    title: "Application Documentation Pack.md",
    path: "01 - Applications/IBX/LA4/Applications/LibreNMS/Application Documentation Pack.md"
  },
  {
    id: "raci",
    title: "10-RACI.md",
    path: "01 - Applications/IBX/LA4/Applications/LibreNMS/10-RACI.md"
  }
];

export const notesById: Record<string, MockNote> = {
  overview: {
    id: "overview",
    title: "Application Documentation Pack",
    path: noteTabs[0].path,
    initialMarkdown: visualMarkdownSpikeInitialMarkdown,
    type: "application-pack",
    status: "Active",
    owner: "Platform Operations",
    reviewer: "SRE Lead",
    version: "0.3",
    created: "2026-06-04",
    updated: "2026-06-04 10:44",
    tags: ["application", "la4", "ops"],
    backlinks: ["05-Monitoring.md", "10-RACI.md", "Restart Service.md"],
    table: [
      { document: "00-Overview", objective: "visao geral da aplicacao", status: "Ready" },
      { document: "05-Monitoring", objective: "sinais, alertas e dashboards", status: "Draft" },
      { document: "10-RACI", objective: "responsabilidades operacionais", status: "Missing" }
    ]
  },
  pack: {
    id: "pack",
    title: "Application Documentation Pack",
    path: noteTabs[1].path,
    initialMarkdown: `# Application Documentation Pack Template

## Purpose

Use this folder-pack template to create operational application documentation with clean Markdown output.

## Required Inputs

- IBX / environment
- Application name
- Owner
- Criticality
- Status
`,
    type: "application-pack",
    status: "Draft",
    owner: "Documentation Guild",
    reviewer: "Operations Lead",
    version: "0.1",
    created: "2026-06-04",
    updated: "2026-06-04 10:44",
    tags: ["template", "application", "governance"],
    backlinks: ["00-Overview.md", "Monthly Review.md"],
    table: [
      { document: "00-Overview", objective: "pack entrypoint", status: "Ready" },
      { document: "01-Technical-Details", objective: "technical reference", status: "Draft" },
      { document: "99-Change-History", objective: "audit trail", status: "Missing" }
    ]
  },
  raci: {
    id: "raci",
    title: "RACI",
    path: noteTabs[2].path,
    initialMarkdown: `# RACI

| Activity | Responsible | Accountable | Consulted | Informed |
|---|---|---|---|---|
| Monitoring ownership | Platform Operations | Application Owner | SRE Lead | Service Desk |
| Incident escalation | NOC | Application Owner | Security | Stakeholders |
`,
    type: "raci",
    status: "Review",
    owner: "Platform Operations",
    reviewer: "Application Owner",
    version: "0.2",
    created: "2026-06-04",
    updated: "2026-06-04 10:44",
    tags: ["raci", "ops", "ownership"],
    backlinks: ["00-Overview.md", "05-Monitoring.md"],
    table: [
      { document: "Responsible", objective: "Platform Operations", status: "Ready" },
      { document: "Accountable", objective: "Application Owner", status: "Draft" },
      { document: "Consulted", objective: "Security", status: "Missing" }
    ]
  }
};

import type { FastifyInstance } from "fastify";
import type { ApiNote, SaveNoteContentBody } from "../types.js";

const notes = new Map<string, ApiNote>([
  [
    "overview",
    {
      id: "overview",
      markdown: `---
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

> Este pacote organiza a documentacao tecnica operacional de uma aplicacao.

## Checklist

- [x] Owner definido
- [ ] RACI revisada
`,
      metadata: {
        owner: "Amer - Monitoring & Tools",
        status: "draft",
        tags: ["application", "la4", "ops"],
        title: "Application Documentation Pack",
        type: "application-pack",
        updated: "2026-06-04"
      },
      path: "01 - Applications/IBX/LA4/Applications/LibreNMS/00-Overview.md"
    }
  ],
  [
    "raci",
    {
      id: "raci",
      markdown: `# RACI

| Activity | Responsible | Accountable | Consulted | Informed |
|---|---|---|---|---|
| Monitoring ownership | Platform Operations | Application Owner | SRE Lead | Service Desk |
`,
      metadata: {
        owner: "Platform Operations",
        status: "review",
        tags: ["raci", "ops"],
        title: "RACI",
        type: "raci",
        updated: "2026-06-04"
      },
      path: "01 - Applications/IBX/LA4/Applications/LibreNMS/10-RACI.md"
    }
  ]
]);

export async function registerNoteRoutes(server: FastifyInstance) {
  server.get("/api/notes", async () => Array.from(notes.values()));

  server.get<{ Params: { id: string } }>("/api/notes/:id", async (request, reply) => {
    const note = notes.get(request.params.id);

    if (!note) {
      return reply.code(404).send({
        error: "not_found",
        message: "Note not found"
      });
    }

    return note;
  });

  server.put<{ Body: SaveNoteContentBody; Params: { id: string } }>(
    "/api/notes/:id/content",
    async (request, reply) => {
      const note = notes.get(request.params.id);

      if (!note) {
        return reply.code(404).send({
          error: "not_found",
          message: "Note not found"
        });
      }

      if (typeof request.body?.markdown !== "string") {
        return reply.code(400).send({
          error: "invalid_body",
          message: "Expected body.markdown to be a string"
        });
      }

      const updatedNote: ApiNote = {
        ...note,
        markdown: request.body.markdown,
        metadata: {
          ...note.metadata,
          updated: new Date().toISOString()
        }
      };

      notes.set(note.id, updatedNote);

      return {
        noteId: note.id,
        savedAt: updatedNote.metadata.updated,
        status: "saved"
      };
    }
  );
}

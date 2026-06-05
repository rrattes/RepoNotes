import type { FastifyInstance } from "fastify";
import { randomUUID } from "node:crypto";
import { getDatabase } from "../db/connection.js";
import type { ApiNote, CreateNoteBody, NoteRow, SaveNoteContentBody, SaveNoteMetadataBody } from "../types.js";

type TagRow = {
  name: string;
};

function combineMarkdown(frontmatter: string | null, body: string) {
  if (!frontmatter?.trim()) {
    return body;
  }

  return `---\n${frontmatter.trim()}\n---\n\n${body}`;
}

function splitMarkdown(markdown: string) {
  if (!markdown.startsWith("---\n")) {
    return {
      body: markdown,
      frontmatter: null
    };
  }

  const closingIndex = markdown.indexOf("\n---", 4);

  if (closingIndex === -1) {
    return {
      body: markdown,
      frontmatter: null
    };
  }

  const frontmatter = markdown.slice(4, closingIndex).trim();
  const bodyStart = closingIndex + "\n---".length;
  const body = markdown.slice(bodyStart).replace(/^\r?\n\r?\n?/, "");

  return {
    body,
    frontmatter
  };
}

function notePath(row: NoteRow) {
  return `notes/${row.title.replace(/[<>:"/\\|?*]+/g, "-")}.md`;
}

function noteIdFromTitle(title: string) {
  const slug = title
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)/g, "");

  return slug || randomUUID();
}

function getUniqueNoteId(preferredId: string) {
  const db = getDatabase();
  let id = preferredId;
  let counter = 2;

  while (db.prepare("SELECT id FROM notes WHERE id = ?").get(id)) {
    id = `${preferredId}-${counter}`;
    counter += 1;
  }

  return id;
}

function toApiNote(row: NoteRow): ApiNote {
  const db = getDatabase();
  const tags = db.prepare(`
    SELECT tags.name
    FROM tags
    INNER JOIN note_tags ON note_tags.tag_id = tags.id
    WHERE note_tags.note_id = ?
    ORDER BY tags.name
  `).all(row.id) as TagRow[];

  return {
    id: row.id,
    markdown: combineMarkdown(row.frontmatter, row.body_markdown),
    metadata: {
      owner: row.owner ?? undefined,
      status: row.status ?? undefined,
      tags: tags.map((tag) => tag.name),
      title: row.title,
      type: row.type ?? undefined,
      updated: row.updated_at
    },
    path: notePath(row)
  };
}

function getNoteRow(id: string) {
  const db = getDatabase();

  return db.prepare(`
    SELECT *
    FROM notes
    WHERE id = ? AND deleted_at IS NULL
  `).get(id) as NoteRow | undefined;
}

function getAnyNoteRow(id: string) {
  const db = getDatabase();

  return db.prepare(`
    SELECT *
    FROM notes
    WHERE id = ?
  `).get(id) as NoteRow | undefined;
}

function insertAuditEvent(eventType: string, entityId: string | null, details: Record<string, unknown> = {}) {
  const db = getDatabase();
  const now = new Date().toISOString();

  db.prepare(`
    INSERT INTO audit_events (id, event_type, entity_type, entity_id, created_at, details_json)
    VALUES (?, ?, ?, ?, ?, ?)
  `).run(
    randomUUID(),
    eventType,
    "note",
    entityId,
    now,
    JSON.stringify(details)
  );
}

export async function registerNoteRoutes(server: FastifyInstance) {
  server.get("/api/notes", async () => {
    const db = getDatabase();
    const rows = db.prepare(`
      SELECT *
      FROM notes
      WHERE deleted_at IS NULL
      ORDER BY updated_at DESC
    `).all() as NoteRow[];

    return rows.map(toApiNote);
  });

  server.post<{ Body: CreateNoteBody }>("/api/notes", async (request, reply) => {
    const db = getDatabase();
    const title = request.body?.title?.trim() || "Untitled Note";
    const requestedId = request.body?.id?.trim() || noteIdFromTitle(title);
    const id = getUniqueNoteId(requestedId);
    const now = new Date().toISOString();
    const bodyMarkdown = request.body?.bodyMarkdown ?? `# ${title}\n`;
    const frontmatter = request.body?.frontmatter?.trim() || null;

    const createNote = db.transaction(() => {
      db.prepare(`
        INSERT INTO notes (
          id,
          title,
          body_markdown,
          frontmatter,
          status,
          type,
          owner,
          created_at,
          updated_at,
          deleted_at
        )
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, NULL)
      `).run(
        id,
        title,
        bodyMarkdown,
        frontmatter,
        request.body?.status ?? null,
        request.body?.type ?? null,
        request.body?.owner ?? null,
        now,
        now
      );

      insertAuditEvent("note.created", id, { source: "api" });
    });

    createNote();

    const note = getNoteRow(id);

    if (!note) {
      return reply.code(500).send({
        error: "create_failed",
        message: "Note was not created"
      });
    }

    return reply.code(201).send(toApiNote(note));
  });

  server.get<{ Params: { id: string } }>("/api/notes/:id", async (request, reply) => {
    const note = getNoteRow(request.params.id);

    if (!note) {
      return reply.code(404).send({
        error: "not_found",
        message: "Note not found"
      });
    }

    return toApiNote(note);
  });

  server.put<{ Body: SaveNoteContentBody; Params: { id: string } }>(
    "/api/notes/:id/content",
    async (request, reply) => {
      const db = getDatabase();
      const note = getNoteRow(request.params.id);

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

      const updatedAt = new Date().toISOString();
      const markdownParts = splitMarkdown(request.body.markdown);

      const updateNote = db.transaction(() => {
        db.prepare(`
          UPDATE notes
          SET body_markdown = ?,
              frontmatter = ?,
              updated_at = ?
          WHERE id = ?
        `).run(markdownParts.body, markdownParts.frontmatter, updatedAt, note.id);

        db.prepare(`
          INSERT INTO audit_events (id, event_type, entity_type, entity_id, created_at, details_json)
          VALUES (?, ?, ?, ?, ?, ?)
        `).run(
          randomUUID(),
          "note.content_saved",
          "note",
          note.id,
          updatedAt,
          JSON.stringify({ source: "api" })
        );
      });

      updateNote();

      return {
        noteId: note.id,
        savedAt: updatedAt,
        status: "saved"
      };
    }
  );

  server.patch<{ Body: SaveNoteMetadataBody; Params: { id: string } }>(
    "/api/notes/:id/metadata",
    async (request, reply) => {
      const db = getDatabase();
      const note = getNoteRow(request.params.id);

      if (!note) {
        return reply.code(404).send({
          error: "not_found",
          message: "Note not found"
        });
      }

      const updatedAt = new Date().toISOString();
      const nextTitle = request.body?.title?.trim() || note.title;

      const updateMetadata = db.transaction(() => {
        db.prepare(`
          UPDATE notes
          SET title = ?,
              type = ?,
              status = ?,
              owner = ?,
              frontmatter = ?,
              updated_at = ?
          WHERE id = ?
        `).run(
          nextTitle,
          request.body?.type ?? note.type,
          request.body?.status ?? note.status,
          request.body?.owner ?? note.owner,
          request.body?.frontmatter ?? note.frontmatter,
          updatedAt,
          note.id
        );

        insertAuditEvent("note.metadata_saved", note.id, { source: "api" });
      });

      updateMetadata();

      const updatedNote = getNoteRow(note.id);

      if (!updatedNote) {
        return reply.code(500).send({
          error: "update_failed",
          message: "Note metadata was not updated"
        });
      }

      return toApiNote(updatedNote);
    }
  );

  server.delete<{ Params: { id: string } }>("/api/notes/:id", async (request, reply) => {
    const db = getDatabase();
    const note = getNoteRow(request.params.id);

    if (!note) {
      return reply.code(404).send({
        error: "not_found",
        message: "Note not found"
      });
    }

    const deletedAt = new Date().toISOString();

    const moveToTrash = db.transaction(() => {
      db.prepare(`
        UPDATE notes
        SET deleted_at = ?,
            updated_at = ?
        WHERE id = ?
      `).run(deletedAt, deletedAt, note.id);

      insertAuditEvent("note.moved_to_trash", note.id, { source: "api" });
    });

    moveToTrash();

    return {
      deletedAt,
      noteId: note.id,
      status: "deleted"
    };
  });

  server.post<{ Params: { id: string } }>("/api/notes/:id/restore", async (request, reply) => {
    const db = getDatabase();
    const note = getAnyNoteRow(request.params.id);

    if (!note) {
      return reply.code(404).send({
        error: "not_found",
        message: "Note not found"
      });
    }

    const restoredAt = new Date().toISOString();

    const restoreNote = db.transaction(() => {
      db.prepare(`
        UPDATE notes
        SET deleted_at = NULL,
            updated_at = ?
        WHERE id = ?
      `).run(restoredAt, note.id);

      insertAuditEvent("note.restored", note.id, { source: "api" });
    });

    restoreNote();

    const restoredNote = getNoteRow(note.id);

    if (!restoredNote) {
      return reply.code(500).send({
        error: "restore_failed",
        message: "Note was not restored"
      });
    }

    return toApiNote(restoredNote);
  });
}

import type { FastifyInstance } from "fastify";
import { randomUUID } from "node:crypto";
import { getDatabase } from "../db/connection.js";
import type { ApiNote, NoteRow, SaveNoteContentBody } from "../types.js";

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
}

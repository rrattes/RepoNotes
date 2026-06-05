import type { Database } from "better-sqlite3";
import { randomUUID } from "node:crypto";
import { getDatabase } from "./connection.js";

const seedFrontmatter = `title: Application Documentation Pack
type: application-pack
status: draft
owner: Amer - Monitoring & Tools
tags:
  - application
  - la4
  - ops`;

const seedBody = `# Application Documentation Pack

> Este pacote organiza a documentacao tecnica operacional de uma aplicacao.

## Checklist

- [x] Owner definido
- [ ] RACI revisada
`;

const seedTags = ["application", "la4", "ops"];

function insertTag(db: Database, name: string) {
  const id = name.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/(^-|-$)/g, "");

  db.prepare("INSERT OR IGNORE INTO tags (id, name) VALUES (?, ?)").run(id, name);

  return id;
}

export function seedDatabase() {
  const db = getDatabase();
  const count = db.prepare("SELECT COUNT(*) AS count FROM notes").get() as { count: number };

  if (count.count > 0) {
    return;
  }

  const now = new Date().toISOString();

  const insertSeed = db.transaction(() => {
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
      "overview",
      "Application Documentation Pack",
      seedBody,
      seedFrontmatter,
      "draft",
      "application-pack",
      "Amer - Monitoring & Tools",
      now,
      now
    );

    for (const tag of seedTags) {
      const tagId = insertTag(db, tag);
      db.prepare("INSERT OR IGNORE INTO note_tags (note_id, tag_id) VALUES (?, ?)").run("overview", tagId);
    }

    db.prepare(`
      INSERT INTO audit_events (id, event_type, entity_type, entity_id, created_at, details_json)
      VALUES (?, ?, ?, ?, ?, ?)
    `).run(
      randomUUID(),
      "seed.created",
      "note",
      "overview",
      now,
      JSON.stringify({ source: "initial-sqlite-seed" })
    );
  });

  insertSeed();
}

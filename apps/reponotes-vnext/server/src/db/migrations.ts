import { existsSync, readFileSync } from "node:fs";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";
import { getDatabase } from "./connection.js";

const currentDirectory = dirname(fileURLToPath(import.meta.url));
const schemaPathCandidates = [
  join(currentDirectory, "schema.sql"),
  resolve("src/db/schema.sql")
];

export function runMigrations() {
  const db = getDatabase();
  const schemaPath = schemaPathCandidates.find((candidate) => existsSync(candidate));

  if (!schemaPath) {
    throw new Error("SQLite schema.sql was not found");
  }

  const schemaSql = readFileSync(schemaPath, "utf8");

  db.exec(schemaSql);

  const currentVersion = db
    .prepare("SELECT version FROM schema_versions WHERE version = ?")
    .get(1);

  if (!currentVersion) {
    db.prepare(
      "INSERT INTO schema_versions (version, applied_at) VALUES (?, ?)"
    ).run(1, new Date().toISOString());
  }
}

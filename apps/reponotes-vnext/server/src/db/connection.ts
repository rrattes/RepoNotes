import Database from "better-sqlite3";
import type { Database as SqliteDatabase } from "better-sqlite3";
import { mkdirSync } from "node:fs";
import { dirname, resolve } from "node:path";

let database: Database.Database | null = null;

export function getDatabase(): SqliteDatabase {
  if (database) {
    return database;
  }

  const dbPath = resolve(
    process.env.REPNOTES_DB_PATH ?? "data/reponotes-dev.db"
  );

  mkdirSync(dirname(dbPath), { recursive: true });

  database = new Database(dbPath);
  database.pragma("foreign_keys = ON");
  database.pragma("journal_mode = WAL");

  return database;
}

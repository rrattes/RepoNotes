import type { MockNote, NoteMetadata } from "../types/reponotes";
import { ApiClientError, getJson } from "./apiClient";
import type { RepositoryService, RepositorySnapshot } from "./RepositoryService";

type ApiNote = {
  id: string;
  markdown: string;
  metadata: NoteMetadata;
  path: string;
};

function toMockNote(note: ApiNote): MockNote {
  return {
    id: note.id,
    title: note.metadata.title ?? note.id,
    path: note.path,
    initialMarkdown: note.markdown,
    type: note.metadata.type ?? "note",
    status: note.metadata.status ?? "Draft",
    owner: note.metadata.owner ?? "",
    reviewer: "",
    version: "0.1",
    created: "",
    updated: note.metadata.updated ?? "",
    tags: note.metadata.tags ?? [],
    backlinks: [],
    table: []
  };
}

class HttpRepositoryService implements RepositoryService {
  async getRepository(): Promise<RepositorySnapshot> {
    const notes = await this.listNotes();

    return {
      id: "infra-docs",
      name: "infra-docs",
      tree: notes.map((note) => ({
        id: note.id,
        name: note.path.split("/").at(-1) ?? note.title,
        type: "note"
      }))
    };
  }

  async listNotes(): Promise<MockNote[]> {
    const notes = await getJson<ApiNote[]>("/api/notes");
    return notes.map(toMockNote);
  }

  async getNoteById(id: string): Promise<MockNote | undefined> {
    try {
      const note = await getJson<ApiNote>(`/api/notes/${encodeURIComponent(id)}`);
      return toMockNote(note);
    } catch (error: unknown) {
      if (error instanceof ApiClientError && error.status === 404) {
        return undefined;
      }

      throw error;
    }
  }

  async getActiveNote(): Promise<MockNote> {
    const notes = await this.listNotes();
    const activeNote = notes[0];

    if (!activeNote) {
      throw new Error("No notes returned by API");
    }

    return activeNote;
  }

  async listTrashItems(): Promise<MockNote[]> {
    return [];
  }
}

export const httpRepositoryService: RepositoryService = new HttpRepositoryService();

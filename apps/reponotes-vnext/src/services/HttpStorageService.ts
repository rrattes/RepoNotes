import type { NoteMetadata } from "../types/reponotes";
import { deleteJson, patchJson, postJson, putJson } from "./apiClient";
import { toMockNote } from "./HttpRepositoryService";
import type { CreateNoteInput, SaveNoteContentResult, StorageService } from "./StorageService";

type SaveNoteContentResponse = {
  noteId: string;
  savedAt: string;
  status: "saved";
};

class HttpStorageService implements StorageService {
  async createNote(input: CreateNoteInput) {
    const note = await postJson<CreateNoteInput, Parameters<typeof toMockNote>[0]>("/api/notes", input);
    return toMockNote(note);
  }

  async saveNoteContent(noteId: string, markdown: string): Promise<SaveNoteContentResult> {
    const result = await putJson<{ markdown: string }, SaveNoteContentResponse>(
      `/api/notes/${encodeURIComponent(noteId)}/content`,
      { markdown }
    );

    return {
      markdown,
      noteId: result.noteId,
      savedAt: result.savedAt
    };
  }

  async saveNoteMetadata(noteId: string, metadata: NoteMetadata): Promise<void> {
    await patchJson<NoteMetadata, Parameters<typeof toMockNote>[0]>(
      `/api/notes/${encodeURIComponent(noteId)}/metadata`,
      metadata
    );
  }

  async moveNoteToTrash(noteId: string): Promise<void> {
    await deleteJson<{ deletedAt: string; noteId: string; status: "deleted" }>(
      `/api/notes/${encodeURIComponent(noteId)}`
    );
  }

  async restoreNote(noteId: string): Promise<void> {
    await postJson<undefined, Parameters<typeof toMockNote>[0]>(
      `/api/notes/${encodeURIComponent(noteId)}/restore`
    );
  }
}

export const httpStorageService: StorageService = new HttpStorageService();

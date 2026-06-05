import type { NoteMetadata } from "../types/reponotes";
import { putJson } from "./apiClient";
import type { SaveNoteContentResult, StorageService } from "./StorageService";

type SaveNoteContentResponse = {
  noteId: string;
  savedAt: string;
  status: "saved";
};

class HttpStorageService implements StorageService {
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

  async saveNoteMetadata(_noteId: string, _metadata: NoteMetadata): Promise<void> {
    throw new Error("HTTP metadata save is not implemented yet");
  }

  async moveNoteToTrash(_noteId: string): Promise<void> {
    throw new Error("HTTP move to trash is not implemented yet");
  }

  async restoreNote(_noteId: string): Promise<void> {
    throw new Error("HTTP restore is not implemented yet");
  }
}

export const httpStorageService: StorageService = new HttpStorageService();

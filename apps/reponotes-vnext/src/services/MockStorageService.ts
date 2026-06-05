import { notesById } from "../data/mockRepository";
import type { NoteMetadata } from "../types/reponotes";
import type { SaveNoteContentResult, StorageService } from "./StorageService";

class MockStorageService implements StorageService {
  private readonly contentByNoteId = new Map<string, string>();
  private readonly metadataByNoteId = new Map<string, NoteMetadata>();
  private readonly trashNoteIds = new Set<string>();

  constructor() {
    Object.values(notesById).forEach((note) => {
      this.contentByNoteId.set(note.id, note.initialMarkdown);
    });
  }

  async saveNoteContent(noteId: string, markdown: string): Promise<SaveNoteContentResult> {
    await this.simulateLatency();

    this.contentByNoteId.set(noteId, markdown);

    return {
      markdown,
      noteId,
      savedAt: new Date().toISOString()
    };
  }

  async saveNoteMetadata(noteId: string, metadata: NoteMetadata): Promise<void> {
    await this.simulateLatency();
    this.metadataByNoteId.set(noteId, metadata);
  }

  async moveNoteToTrash(noteId: string): Promise<void> {
    await this.simulateLatency();
    this.trashNoteIds.add(noteId);
  }

  async restoreNote(noteId: string): Promise<void> {
    await this.simulateLatency();
    this.trashNoteIds.delete(noteId);
  }

  getSavedNoteContent(noteId: string): string | undefined {
    return this.contentByNoteId.get(noteId);
  }

  private async simulateLatency() {
    await new Promise((resolve) => {
      window.setTimeout(resolve, 220);
    });
  }
}

export const mockStorageService = new MockStorageService();

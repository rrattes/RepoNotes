import type { NoteMetadata } from "../types/reponotes";

export type SaveNoteContentResult = {
  markdown: string;
  noteId: string;
  savedAt: string;
};

export interface StorageService {
  saveNoteContent(noteId: string, markdown: string): Promise<SaveNoteContentResult>;
  saveNoteMetadata(noteId: string, metadata: NoteMetadata): Promise<void>;
  moveNoteToTrash(noteId: string): Promise<void>;
  restoreNote(noteId: string): Promise<void>;
}

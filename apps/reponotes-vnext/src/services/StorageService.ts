import type { MockNote, NoteMetadata } from "../types/reponotes";

export type CreateNoteInput = {
  bodyMarkdown?: string;
  frontmatter?: string | null;
  id?: string;
  owner?: string | null;
  status?: string | null;
  title: string;
  type?: string | null;
};

export type SaveNoteContentResult = {
  markdown: string;
  noteId: string;
  savedAt: string;
};

export interface StorageService {
  createNote(input: CreateNoteInput): Promise<MockNote>;
  saveNoteContent(noteId: string, markdown: string): Promise<SaveNoteContentResult>;
  saveNoteMetadata(noteId: string, metadata: NoteMetadata): Promise<void>;
  moveNoteToTrash(noteId: string): Promise<void>;
  restoreNote(noteId: string): Promise<void>;
}

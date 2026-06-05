import { notesById } from "../data/mockRepository";
import type { MockNote, NoteMetadata } from "../types/reponotes";
import type { CreateNoteInput, SaveNoteContentResult, StorageService } from "./StorageService";

class MockStorageService implements StorageService {
  private readonly contentByNoteId = new Map<string, string>();
  private readonly metadataByNoteId = new Map<string, NoteMetadata>();
  private readonly trashedNotesById = new Map<string, MockNote>();
  private readonly trashNoteIds = new Set<string>();

  constructor() {
    Object.values(notesById).forEach((note) => {
      this.contentByNoteId.set(note.id, note.initialMarkdown);
    });
  }

  async createNote(input: CreateNoteInput): Promise<MockNote> {
    await this.simulateLatency();

    const id = input.id ?? input.title.toLowerCase().replace(/[^a-z0-9]+/g, "-").replace(/(^-|-$)/g, "");
    const now = new Date().toISOString();
    const initialMarkdown = input.frontmatter
      ? `---\n${input.frontmatter.trim()}\n---\n\n${input.bodyMarkdown ?? `# ${input.title}\n`}`
      : input.bodyMarkdown ?? `# ${input.title}\n`;

    const note: MockNote = {
      backlinks: [],
      created: now,
      id,
      initialMarkdown,
      owner: input.owner ?? "",
      path: `notes/${input.title}.md`,
      reviewer: "",
      status: input.status ?? "Draft",
      table: [],
      tags: [],
      title: input.title,
      type: input.type ?? "note",
      updated: now,
      version: "0.1"
    };

    notesById[id] = note;
    this.contentByNoteId.set(id, initialMarkdown);
    this.metadataByNoteId.set(id, {
      owner: note.owner,
      status: note.status,
      title: note.title,
      type: note.type,
      updated: note.updated
    });

    return note;
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
    const note = notesById[noteId];

    if (note) {
      this.trashedNotesById.set(noteId, note);
      delete notesById[noteId];
    }

    this.trashNoteIds.add(noteId);
  }

  async restoreNote(noteId: string): Promise<void> {
    await this.simulateLatency();
    const note = this.trashedNotesById.get(noteId);

    if (note) {
      notesById[noteId] = note;
      this.trashedNotesById.delete(noteId);
    }

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

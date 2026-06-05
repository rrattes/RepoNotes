import { httpRepositoryService } from "./HttpRepositoryService";
import { httpStorageService } from "./HttpStorageService";
import { mockRepositoryService } from "./MockRepositoryService";
import { mockStorageService } from "./MockStorageService";
import type { RepositoryService } from "./RepositoryService";
import type { StorageService } from "./StorageService";

export type ServiceConnectionStatus = "mock" | "connected" | "offline-fallback";

const viteEnv = (import.meta as ImportMeta & { env?: { VITE_REPONOTES_USE_HTTP?: string } }).env;

export const USE_HTTP_SERVICES = viteEnv?.VITE_REPONOTES_USE_HTTP === "true";

let connectionStatus: ServiceConnectionStatus = USE_HTTP_SERVICES ? "connected" : "mock";
let isApiUnavailable = false;
const listeners = new Set<(status: ServiceConnectionStatus) => void>();

export function getServiceConnectionStatus(): ServiceConnectionStatus {
  return connectionStatus;
}

export function subscribeServiceConnectionStatus(listener: (status: ServiceConnectionStatus) => void) {
  listeners.add(listener);

  return () => {
    listeners.delete(listener);
  };
}

function setServiceConnectionStatus(status: ServiceConnectionStatus) {
  if (connectionStatus === status) {
    return;
  }

  connectionStatus = status;
  listeners.forEach((listener) => listener(status));
}

function markApiConnected() {
  if (USE_HTTP_SERVICES) {
    isApiUnavailable = false;
    setServiceConnectionStatus("connected");
  }
}

function markApiOfflineFallback(error: unknown) {
  if (USE_HTTP_SERVICES) {
    isApiUnavailable = true;
    console.warn("RepoNotes API unavailable; falling back to mock services", error);
    setServiceConnectionStatus("offline-fallback");
  }
}

class FallbackRepositoryService implements RepositoryService {
  async getRepository() {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockRepositoryService.getRepository();
    }

    try {
      const result = await httpRepositoryService.getRepository();
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockRepositoryService.getRepository();
    }
  }

  async listNotes() {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockRepositoryService.listNotes();
    }

    try {
      const result = await httpRepositoryService.listNotes();
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockRepositoryService.listNotes();
    }
  }

  async getNoteById(id: string) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockRepositoryService.getNoteById(id);
    }

    try {
      const result = await httpRepositoryService.getNoteById(id);
      markApiConnected();
      return result ?? mockRepositoryService.getNoteById(id);
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockRepositoryService.getNoteById(id);
    }
  }

  async getActiveNote() {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockRepositoryService.getActiveNote();
    }

    try {
      const result = await httpRepositoryService.getActiveNote();
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockRepositoryService.getActiveNote();
    }
  }

  async listTrashItems() {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockRepositoryService.listTrashItems();
    }

    try {
      const result = await httpRepositoryService.listTrashItems();
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockRepositoryService.listTrashItems();
    }
  }
}

class FallbackStorageService implements StorageService {
  async createNote(input: Parameters<StorageService["createNote"]>[0]) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockStorageService.createNote(input);
    }

    try {
      const result = await httpStorageService.createNote(input);
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockStorageService.createNote(input);
    }
  }

  async saveNoteContent(noteId: string, markdown: string) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockStorageService.saveNoteContent(noteId, markdown);
    }

    try {
      const result = await httpStorageService.saveNoteContent(noteId, markdown);
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockStorageService.saveNoteContent(noteId, markdown);
    }
  }

  async saveNoteMetadata(noteId: string, metadata: Parameters<StorageService["saveNoteMetadata"]>[1]) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockStorageService.saveNoteMetadata(noteId, metadata);
    }

    try {
      const result = await httpStorageService.saveNoteMetadata(noteId, metadata);
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockStorageService.saveNoteMetadata(noteId, metadata);
    }
  }

  async moveNoteToTrash(noteId: string) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockStorageService.moveNoteToTrash(noteId);
    }

    try {
      const result = await httpStorageService.moveNoteToTrash(noteId);
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockStorageService.moveNoteToTrash(noteId);
    }
  }

  async restoreNote(noteId: string) {
    if (!USE_HTTP_SERVICES || isApiUnavailable) {
      return mockStorageService.restoreNote(noteId);
    }

    try {
      const result = await httpStorageService.restoreNote(noteId);
      markApiConnected();
      return result;
    } catch (error: unknown) {
      markApiOfflineFallback(error);
      return mockStorageService.restoreNote(noteId);
    }
  }
}

export const repositoryService: RepositoryService = new FallbackRepositoryService();
export const storageService: StorageService = new FallbackStorageService();

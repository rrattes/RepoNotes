import { notesById, repositoryTree } from "../data/mockRepository";
import type { MockNote } from "../types/reponotes";
import type { RepositoryService, RepositorySnapshot } from "./RepositoryService";

class MockRepositoryService implements RepositoryService {
  async getRepository(): Promise<RepositorySnapshot> {
    return {
      id: "infra-docs",
      name: "infra-docs",
      tree: repositoryTree
    };
  }

  async listNotes(): Promise<MockNote[]> {
    return Object.values(notesById);
  }

  async getNoteById(id: string): Promise<MockNote | undefined> {
    return notesById[id];
  }

  async getActiveNote(): Promise<MockNote> {
    return notesById.overview;
  }

  async listTrashItems(): Promise<MockNote[]> {
    return [];
  }
}

export const mockRepositoryService: RepositoryService = new MockRepositoryService();

import type { MockNote, RepositoryNode } from "../types/reponotes";

export type RepositorySnapshot = {
  id: string;
  name: string;
  tree: RepositoryNode[];
};

export interface RepositoryService {
  getRepository(): Promise<RepositorySnapshot>;
  listNotes(): Promise<MockNote[]>;
  getNoteById(id: string): Promise<MockNote | undefined>;
  getActiveNote(): Promise<MockNote>;
  listTrashItems(): Promise<MockNote[]>;
}

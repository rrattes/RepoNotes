export type RailItemId =
  | "files"
  | "search"
  | "links"
  | "tags"
  | "tasks"
  | "templates"
  | "entities"
  | "trash"
  | "settings"
  | "profile";

export type RepositoryNode = {
  id: string;
  name: string;
  type: "folder" | "note";
  children?: RepositoryNode[];
};

export type NoteTab = {
  id: string;
  title: string;
  path: string;
};

export type DocumentRow = {
  document: string;
  objective: string;
  status: "Ready" | "Draft" | "Missing";
};

export type AutosaveStatus = "saved" | "changed" | "saving" | "error";

export type NoteMetadata = {
  owner?: string;
  status?: string;
  tags?: string[];
  title?: string;
  type?: string;
  updated?: string;
};

export type MockNote = {
  id: string;
  title: string;
  path: string;
  initialMarkdown: string;
  type: string;
  status: string;
  owner: string;
  reviewer: string;
  version: string;
  created: string;
  updated: string;
  tags: string[];
  backlinks: string[];
  table: DocumentRow[];
};

export type ApiNoteMetadata = {
  owner?: string;
  status?: string;
  tags?: string[];
  title?: string;
  type?: string;
  updated?: string;
};

export type ApiNote = {
  id: string;
  path: string;
  markdown: string;
  metadata: ApiNoteMetadata;
};

export type SaveNoteContentBody = {
  markdown: string;
};

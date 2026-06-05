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

export type NoteRow = {
  body_markdown: string;
  created_at: string;
  deleted_at: string | null;
  frontmatter: string | null;
  id: string;
  owner: string | null;
  status: string | null;
  title: string;
  type: string | null;
  updated_at: string;
};

export type CreateNoteBody = {
  bodyMarkdown?: string;
  frontmatter?: string | null;
  id?: string;
  owner?: string | null;
  status?: string | null;
  title?: string;
  type?: string | null;
};

export type SaveNoteMetadataBody = {
  frontmatter?: string | null;
  owner?: string | null;
  status?: string | null;
  title?: string;
  type?: string | null;
};

export type SaveNoteContentBody = {
  markdown: string;
};

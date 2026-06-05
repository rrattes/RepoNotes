import type { MockNote, RailItemId } from "../../types/reponotes";

type RepositorySidebarProps = {
  activeNoteId: string;
  activeRailItem: RailItemId;
  notes: MockNote[];
  onCreateNote: () => void;
  onSelectNote: (noteId: string) => void;
};

const sidebarContext: Record<RailItemId, { label: string; title: string; description: string }> = {
  files: {
    label: "Repository",
    title: "infra-docs",
    description: "Local repository"
  },
  search: {
    label: "Search",
    title: "Search workspace",
    description: "Visual placeholder"
  },
  links: {
    label: "Links",
    title: "Links graph",
    description: "Future area"
  },
  tags: {
    label: "Tags",
    title: "Tags",
    description: "Future area"
  },
  tasks: {
    label: "Tasks",
    title: "Tasks",
    description: "Future area"
  },
  templates: {
    label: "Templates",
    title: "Templates",
    description: "Future area"
  },
  entities: {
    label: "Entities",
    title: "Entities",
    description: "Future area"
  },
  trash: {
    label: "Trash",
    title: "Trash",
    description: "Hidden for now"
  },
  settings: {
    label: "Settings",
    title: "Workspace settings",
    description: "Visual placeholder"
  },
  profile: {
    label: "Profile",
    title: "Profile",
    description: "Future area"
  }
};

export default function RepositorySidebar({
  activeNoteId,
  activeRailItem,
  notes,
  onCreateNote,
  onSelectNote
}: RepositorySidebarProps) {
  const context = sidebarContext[activeRailItem];

  return (
    <aside className="repository-sidebar">
      <section className="repository-card">
        <div>
          <span className="label">{context.label}</span>
          <strong>{context.title}</strong>
        </div>
        <span className="local-pill">{context.description}</span>
      </section>

      <button className="sidebar-action-button" onClick={onCreateNote} type="button">
        New note
      </button>

      <label className="file-search">
        <span className="sr-only">Buscar arquivo</span>
        <input placeholder="Buscar arquivo..." />
      </label>

      <nav className="repository-tree" aria-label="repository notes">
        {notes.length === 0 ? (
          <p className="sidebar-empty">No notes yet.</p>
        ) : (
          notes.map((note) => (
            <button
              className={`tree-node note ${note.id === activeNoteId ? "active" : ""}`}
              key={note.id}
              onClick={() => onSelectNote(note.id)}
              title={note.path}
              type="button"
            >
              <span className="tree-glyph">MD</span>
              <span>{note.title}</span>
            </button>
          ))
        )}
      </nav>

      <section className="trash-summary">
        <span>Trash</span>
        <strong>Hidden</strong>
      </section>
    </aside>
  );
}

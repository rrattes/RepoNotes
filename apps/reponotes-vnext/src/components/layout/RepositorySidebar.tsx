import { repositoryTree } from "../../data/mockRepository";
import type { RailItemId, RepositoryNode } from "../../types/reponotes";
import type { CSSProperties } from "react";

type RepositorySidebarProps = {
  activeNoteId: string;
  activeRailItem: RailItemId;
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
    description: "2 items"
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

export default function RepositorySidebar({ activeNoteId, activeRailItem, onSelectNote }: RepositorySidebarProps) {
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

      <label className="file-search">
        <span className="sr-only">Buscar arquivo</span>
        <input placeholder="Buscar arquivo..." />
      </label>

      <nav className="repository-tree" aria-label="repository tree">
        {repositoryTree.map((node) => (
          <TreeNode key={node.id} activeNoteId={activeNoteId} node={node} onSelectNote={onSelectNote} />
        ))}
      </nav>

      <section className="trash-summary">
        <span>Trash</span>
        <strong>2 items</strong>
      </section>
    </aside>
  );
}

function TreeNode({
  node,
  depth = 0,
  activeNoteId,
  onSelectNote
}: {
  node: RepositoryNode;
  depth?: number;
  activeNoteId: string;
  onSelectNote: (noteId: string) => void;
}) {
  const isActive = node.id === activeNoteId;
  const isNote = node.type === "note";

  return (
    <div>
      <button
        className={`tree-node ${node.type} ${isActive ? "active" : ""}`}
        onClick={() => isNote && onSelectNote(node.id)}
        style={{ "--depth": depth } as CSSProperties}
        type="button"
      >
        <span className="tree-glyph">{isNote ? "MD" : "▾"}</span>
        <span>{node.name}</span>
      </button>
      {node.children?.map((child) => (
        <TreeNode
          key={child.id}
          activeNoteId={activeNoteId}
          depth={depth + 1}
          node={child}
          onSelectNote={onSelectNote}
        />
      ))}
    </div>
  );
}

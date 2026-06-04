import { repositoryTree } from "../../data/mockRepository";
import type { RepositoryNode } from "../../types/reponotes";
import type { CSSProperties } from "react";

type RepositorySidebarProps = {
  activeNoteId: string;
  onSelectNote: (noteId: string) => void;
};

export default function RepositorySidebar({ activeNoteId, onSelectNote }: RepositorySidebarProps) {
  return (
    <aside className="repository-sidebar">
      <section className="repository-card">
        <div>
          <span className="label">Repository</span>
          <strong>infra-docs</strong>
        </div>
        <span className="local-pill">Local repository</span>
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

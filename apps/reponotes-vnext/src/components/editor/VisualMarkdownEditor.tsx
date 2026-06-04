import { useEffect, useRef, useState } from "react";
import { Crepe } from "@milkdown/crepe";
import "@milkdown/crepe/theme/frame-dark.css";

import type { MockNote } from "../../types/reponotes";

type VisualMarkdownEditorProps = {
  note: MockNote;
};

export default function VisualMarkdownEditor({ note }: VisualMarkdownEditorProps) {
  const editorRootRef = useRef<HTMLDivElement | null>(null);
  const crepeRef = useRef<Crepe | null>(null);
  const [markdown, setMarkdown] = useState(note.initialMarkdown);

  useEffect(() => {
    setMarkdown(note.initialMarkdown);
  }, [note.id, note.initialMarkdown]);

  useEffect(() => {
    const root = editorRootRef.current;

    if (!root) {
      return;
    }

    root.innerHTML = "";

    const crepe = new Crepe({
      root,
      defaultValue: note.initialMarkdown,
      features: {
        [Crepe.Feature.AI]: false,
        [Crepe.Feature.ImageBlock]: false,
        [Crepe.Feature.Latex]: false,
        [Crepe.Feature.TopBar]: false
      },
      featureConfigs: {
        [Crepe.Feature.Placeholder]: {
          text: "Digite Markdown aqui..."
        }
      }
    });

    crepe.on((listener) => {
      listener.markdownUpdated((_, nextMarkdown) => {
        setMarkdown(nextMarkdown);
      });
    });

    crepeRef.current = crepe;
    crepe.create().then(() => {
      setMarkdown(crepe.getMarkdown());
    }).catch((error: unknown) => {
      console.error("Failed to create Milkdown editor", error);
    });

    return () => {
      crepeRef.current = null;
      crepe.destroy().catch((error: unknown) => {
        console.error("Failed to destroy Milkdown editor", error);
      });
    };
  }, [note.id, note.initialMarkdown]);

  const hasGeneratedMarkdown = markdown.trim().length > 0;

  return (
    <article className="visual-editor visual-editor-spike" aria-label="visual markdown editor spike">
      <header className="spike-header">
        <div>
          <div className="document-badges">
            <span>{note.type}</span>
            <span>{note.status}</span>
            <span>owner: {note.owner}</span>
          </div>
          <h1>Visual Markdown Editor Spike</h1>
          <p>
            Milkdown/Crepe editor visual em memoria. O Markdown gerado abaixo e a fonte limpa que seria salva em
            rodadas futuras.
          </p>
        </div>
        <div className={`spike-verdict ${hasGeneratedMarkdown ? "ok" : "warning"}`}>
          <span>{hasGeneratedMarkdown ? "Markdown round-trip ativo" : "Aguardando Markdown"}</span>
        </div>
      </header>

      <section className="milkdown-shell" aria-label="Milkdown visual editor">
        <div ref={editorRootRef} />
      </section>

      <section className="markdown-debug-panel" aria-label="generated markdown debug panel">
        <header>
          <span>Markdown gerado</span>
          <strong>{markdown.length} chars</strong>
        </header>
        <pre>{markdown}</pre>
      </section>
    </article>
  );
}

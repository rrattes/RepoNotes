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

  return (
    <article
      className="visual-editor visual-editor-main"
      aria-label="visual markdown editor"
      data-generated-markdown-length={markdown.length}
    >
      <section className="milkdown-shell" aria-label="Milkdown visual editor">
        <div ref={editorRootRef} />
      </section>
    </article>
  );
}

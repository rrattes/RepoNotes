import { useEffect, useRef, useState } from "react";
import { Crepe } from "@milkdown/crepe";
import "@milkdown/crepe/theme/frame-dark.css";

import type { MockNote } from "../../types/reponotes";

type VisualMarkdownEditorProps = {
  note: MockNote;
};

type MarkdownParts = {
  body: string;
  frontmatter: string;
};

const frontmatterPattern = /^---\r?\n[\s\S]*?\r?\n---(?:\r?\n|$)/;

function splitMarkdownFrontmatter(markdown: string): MarkdownParts {
  const match = markdown.match(frontmatterPattern);

  if (!match) {
    return {
      body: markdown,
      frontmatter: ""
    };
  }

  return {
    body: markdown.slice(match[0].length),
    frontmatter: match[0].trimEnd()
  };
}

function combineMarkdown(frontmatter: string, body: string) {
  if (!frontmatter) {
    return body;
  }

  return `${frontmatter}\n\n${body}`;
}

const editorGutterNumbers = Array.from({ length: 36 }, (_, index) => index + 1);

export default function VisualMarkdownEditor({ note }: VisualMarkdownEditorProps) {
  const editorRootRef = useRef<HTMLDivElement | null>(null);
  const crepeRef = useRef<Crepe | null>(null);
  const [markdownParts, setMarkdownParts] = useState(() => splitMarkdownFrontmatter(note.initialMarkdown));
  const [markdown, setMarkdown] = useState(() => {
    const initialParts = splitMarkdownFrontmatter(note.initialMarkdown);
    return combineMarkdown(initialParts.frontmatter, initialParts.body);
  });

  useEffect(() => {
    const nextParts = splitMarkdownFrontmatter(note.initialMarkdown);
    setMarkdownParts(nextParts);
    setMarkdown(combineMarkdown(nextParts.frontmatter, nextParts.body));
  }, [note.id, note.initialMarkdown]);

  useEffect(() => {
    const root = editorRootRef.current;

    if (!root) {
      return;
    }

    root.innerHTML = "";

    const crepe = new Crepe({
      root,
      defaultValue: markdownParts.body,
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
        setMarkdown(combineMarkdown(markdownParts.frontmatter, nextMarkdown));
      });
    });

    crepeRef.current = crepe;
    crepe.create().then(() => {
      setMarkdown(combineMarkdown(markdownParts.frontmatter, crepe.getMarkdown()));
    }).catch((error: unknown) => {
      console.error("Failed to create Milkdown editor", error);
    });

    return () => {
      crepeRef.current = null;
      crepe.destroy().catch((error: unknown) => {
        console.error("Failed to destroy Milkdown editor", error);
      });
    };
  }, [note.id, note.initialMarkdown, markdownParts.body, markdownParts.frontmatter]);

  return (
    <article
      className="visual-editor visual-editor-main"
      aria-label="visual markdown editor"
      data-generated-markdown-length={markdown.length}
      data-has-frontmatter={markdownParts.frontmatter ? "true" : "false"}
      data-frontmatter-boundary={markdownParts.frontmatter ? "body-only-editor" : "none"}
      data-generated-markdown-starts-with-frontmatter={markdown.startsWith("---\n") ? "true" : "false"}
    >
      <aside className="editor-gutter" aria-hidden="true">
        {editorGutterNumbers.map((number) => (
          <span key={number}>{number}</span>
        ))}
      </aside>
      <section className="milkdown-shell" aria-label="Milkdown visual editor">
        <div ref={editorRootRef} />
      </section>
    </article>
  );
}

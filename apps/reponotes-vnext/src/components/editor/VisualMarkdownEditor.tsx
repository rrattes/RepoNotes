import { useEffect, useRef, useState } from "react";
import { Crepe } from "@milkdown/crepe";
import "@milkdown/crepe/theme/frame-dark.css";

import { storageService } from "../../services/serviceRegistry";
import type { AutosaveStatus, MockNote } from "../../types/reponotes";
import { combineMarkdownFrontmatter, splitMarkdownFrontmatter } from "./markdownFrontmatter";

type VisualMarkdownEditorProps = {
  note: MockNote;
  onAutosaveStatusChange: (status: AutosaveStatus) => void;
};

type EditorDebugOptions = {
  debugLayout: boolean;
  flatEditor: boolean;
};

type LayoutDebugMeasurement = {
  className: string;
  element: string;
  fontSize: string;
  height: number;
  lineHeight: string;
  marginLeft: string;
  marginTop: string;
  paddingLeft: string;
  paddingTop: string;
  scrollLeft: number;
  scrollTop: number;
  transform: string;
  width: number;
  x: number;
  y: number;
};

function getEditorDebugOptions(): EditorDebugOptions {
  const isDev = (import.meta as ImportMeta & { env?: { DEV?: boolean } }).env?.DEV === true;

  if (!isDev || typeof window === "undefined") {
    return {
      debugLayout: false,
      flatEditor: false
    };
  }

  const params = new URLSearchParams(window.location.search);

  return {
    debugLayout: params.get("debugLayout") === "1",
    flatEditor: params.get("flatEditor") === "1"
  };
}

function measureElement(element: Element | null, label: string): LayoutDebugMeasurement {
  if (!element) {
    return {
      className: "",
      element: label,
      fontSize: "",
      height: 0,
      lineHeight: "",
      marginLeft: "",
      marginTop: "",
      paddingLeft: "",
      paddingTop: "",
      scrollLeft: 0,
      scrollTop: 0,
      transform: "",
      width: 0,
      x: 0,
      y: 0
    };
  }

  const rect = element.getBoundingClientRect();
  const styles = window.getComputedStyle(element);

  return {
    className: element.className.toString(),
    element: label,
    fontSize: styles.fontSize,
    height: Number(rect.height.toFixed(2)),
    lineHeight: styles.lineHeight,
    marginLeft: styles.marginLeft,
    marginTop: styles.marginTop,
    paddingLeft: styles.paddingLeft,
    paddingTop: styles.paddingTop,
    scrollLeft: element.scrollLeft,
    scrollTop: element.scrollTop,
    transform: styles.transform,
    width: Number(rect.width.toFixed(2)),
    x: Number(rect.x.toFixed(2)),
    y: Number(rect.y.toFixed(2))
  };
}

function getLayoutDebugMeasurements(): LayoutDebugMeasurement[] {
  return [
    measureElement(document.querySelector(".milkdown-shell"), "milkdown-shell"),
    measureElement(document.querySelector(".milkdown-shell .ProseMirror"), "ProseMirror"),
    measureElement(document.querySelector(".milkdown-shell .ProseMirror h1"), "first-h1"),
    measureElement(document.querySelector(".milkdown-shell .ProseMirror p"), "first-p"),
    measureElement(document.querySelector(".milkdown-shell .ProseMirror blockquote"), "first-blockquote")
  ];
}

export default function VisualMarkdownEditor({ note, onAutosaveStatusChange }: VisualMarkdownEditorProps) {
  const editorRootRef = useRef<HTMLDivElement | null>(null);
  const crepeRef = useRef<Crepe | null>(null);
  const [debugOptions] = useState(getEditorDebugOptions);
  const [layoutMeasurements, setLayoutMeasurements] = useState<LayoutDebugMeasurement[]>([]);
  const [markdownParts, setMarkdownParts] = useState(() => splitMarkdownFrontmatter(note.initialMarkdown));
  const [savedMarkdown, setSavedMarkdown] = useState(() => note.initialMarkdown);
  const [markdown, setMarkdown] = useState(() => {
    const initialParts = splitMarkdownFrontmatter(note.initialMarkdown);
    return combineMarkdownFrontmatter(initialParts.frontmatter, initialParts.body);
  });

  useEffect(() => {
    const nextParts = splitMarkdownFrontmatter(note.initialMarkdown);
    setMarkdownParts(nextParts);
    const recomposedMarkdown = combineMarkdownFrontmatter(nextParts.frontmatter, nextParts.body);
    setMarkdown(recomposedMarkdown);
    setSavedMarkdown(recomposedMarkdown);
    onAutosaveStatusChange("saved");
  }, [note.id, note.initialMarkdown, onAutosaveStatusChange]);

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
        setMarkdown(combineMarkdownFrontmatter(markdownParts.frontmatter, nextMarkdown));
      });
    });

    crepeRef.current = crepe;
    crepe.create().then(() => {
      setMarkdown(combineMarkdownFrontmatter(markdownParts.frontmatter, crepe.getMarkdown()));
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

  useEffect(() => {
    if (markdown === savedMarkdown) {
      onAutosaveStatusChange("saved");
      return;
    }

    onAutosaveStatusChange("changed");

    const debounceTimer = window.setTimeout(() => {
      onAutosaveStatusChange("saving");

      storageService.saveNoteContent(note.id, markdown)
        .then((result) => {
          setSavedMarkdown(result.markdown);
          onAutosaveStatusChange("saved");
        })
        .catch((error: unknown) => {
          console.error("Failed to autosave Markdown", error);
          onAutosaveStatusChange("error");
        });
    }, 650);

    return () => {
      window.clearTimeout(debounceTimer);
    };
  }, [markdown, note.id, onAutosaveStatusChange, savedMarkdown]);

  useEffect(() => {
    if (!debugOptions.debugLayout) {
      return;
    }

    let animationFrame = 0;

    const captureLayout = () => {
      window.cancelAnimationFrame(animationFrame);
      animationFrame = window.requestAnimationFrame(() => {
        const nextMeasurements = getLayoutDebugMeasurements();
        setLayoutMeasurements(nextMeasurements);
        console.table(nextMeasurements);
      });
    };

    captureLayout();

    document.addEventListener("click", captureLayout, true);
    document.addEventListener("focusin", captureLayout, true);
    document.addEventListener("keyup", captureLayout, true);
    document.addEventListener("mouseup", captureLayout, true);
    document.addEventListener("selectionchange", captureLayout);
    window.addEventListener("resize", captureLayout);

    return () => {
      window.cancelAnimationFrame(animationFrame);
      document.removeEventListener("click", captureLayout, true);
      document.removeEventListener("focusin", captureLayout, true);
      document.removeEventListener("keyup", captureLayout, true);
      document.removeEventListener("mouseup", captureLayout, true);
      document.removeEventListener("selectionchange", captureLayout);
      window.removeEventListener("resize", captureLayout);
    };
  }, [debugOptions.debugLayout]);

  const editorClassName = [
    "visual-editor visual-editor-main",
    debugOptions.debugLayout ? "debug-layout" : "",
    debugOptions.flatEditor ? "debug-flat-editor" : ""
  ].filter(Boolean).join(" ");

  return (
    <article
      className={editorClassName}
      aria-label="visual markdown editor"
      data-generated-markdown-length={markdown.length}
      data-has-frontmatter={markdownParts.frontmatter ? "true" : "false"}
      data-frontmatter-boundary={markdownParts.frontmatter ? "body-only-editor" : "none"}
      data-generated-markdown-starts-with-frontmatter={markdown.startsWith("---\n") ? "true" : "false"}
    >
      <section className="milkdown-shell" aria-label="Milkdown visual editor">
        <div ref={editorRootRef} />
      </section>
      {debugOptions.debugLayout ? (
        <aside className="layout-debug-overlay" aria-label="Editor layout debug">
          <strong>Layout debug</strong>
          <span>flatEditor={debugOptions.flatEditor ? "1" : "0"}</span>
          <table>
            <thead>
              <tr>
                <th>el</th>
                <th>x</th>
                <th>y</th>
                <th>w</th>
                <th>h</th>
                <th>ml</th>
                <th>mt</th>
                <th>pl</th>
                <th>pt</th>
                <th>sl</th>
                <th>st</th>
              </tr>
            </thead>
            <tbody>
              {layoutMeasurements.map((measurement) => (
                <tr key={measurement.element}>
                  <td title={measurement.className}>{measurement.element}</td>
                  <td>{measurement.x}</td>
                  <td>{measurement.y}</td>
                  <td>{measurement.width}</td>
                  <td>{measurement.height}</td>
                  <td>{measurement.marginLeft}</td>
                  <td>{measurement.marginTop}</td>
                  <td>{measurement.paddingLeft}</td>
                  <td>{measurement.paddingTop}</td>
                  <td>{measurement.scrollLeft}</td>
                  <td>{measurement.scrollTop}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </aside>
      ) : null}
    </article>
  );
}

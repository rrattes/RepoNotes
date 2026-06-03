import React, { useMemo, useState } from "react";
import { createRoot } from "react-dom/client";
import "./styles.css";

type ViewMode = "editor" | "preview" | "split";
type SplitPreset = "50/50" | "60/40" | "70/30";

const markdown = `# Application Documentation Pack

Welcome to the **Application Documentation Pack**.
This pack contains everything you need to understand, run, operate, and extend the application.

## What's included

- [x] System overview
- [x] Architecture diagram
- [ ] Configuration reference
- [ ] Operational runbooks

## Overview

The application is built with **Node.js**, **TypeScript**, and **PostgreSQL**.

> Keep this document up to date as the system evolves.`;

const lines = markdown.split("\n");

const tabs = ["00-Overview.md", "01-Technical-Details.md", "10-RACI.md"];
const splitMap: Record<SplitPreset, string> = {
  "50/50": "1fr 10px 1fr",
  "60/40": "3fr 10px 2fr",
  "70/30": "7fr 10px 3fr"
};

function App() {
  const [mode, setMode] = useState<ViewMode>("split");
  const [splitPreset, setSplitPreset] = useState<SplitPreset>("60/40");
  const splitColumns = useMemo(() => splitMap[splitPreset], [splitPreset]);

  return (
    <main className="app-shell">
      <WindowBar />
      <section className="workspace">
        <Sidebar />
        <section className="document-column">
          <DocumentTopBar mode={mode} setMode={setMode} splitPreset={splitPreset} setSplitPreset={setSplitPreset} />
          {mode !== "preview" && <MarkdownToolbar />}
          <section className={`document-surface mode-${mode}`} style={mode === "split" ? { gridTemplateColumns: splitColumns } : undefined}>
            {(mode === "editor" || mode === "split") && <EditorPanel />}
            {mode === "split" && <div className="split-separator" />}
            {(mode === "preview" || mode === "split") && <PreviewPanel />}
          </section>
        </section>
        <RightPanel />
      </section>
      <StatusBar />
    </main>
  );
}

function WindowBar() {
  return (
    <header className="window-bar">
      <div className="brand-mark">R</div>
      <div className="brand-text">RepoNotes</div>
      <div className="window-context">infra-docs / IBX / LA4 / Applications / LibreNMS</div>
      <div className="window-controls" aria-label="mocked window controls">
        <button aria-label="minimize" />
        <button aria-label="maximize" />
        <button aria-label="close" className="danger" />
      </div>
    </header>
  );
}

function Sidebar() {
  return (
    <aside className="sidebar">
      <section className="repo-card">
        <div>
          <span className="eyebrow">Repository</span>
          <strong>infra-docs</strong>
        </div>
        <span className="sync-dot">local</span>
      </section>
      <div className="search-box">Search notes... <kbd>Ctrl K</kbd></div>
      <div className="side-actions">
        <button className="primary">New Note</button>
        <button>New Folder</button>
      </div>
      <nav className="explorer" aria-label="mock repository tree">
        <TreeFolder label="IBX" depth={0} />
        <TreeFolder label="LA4" depth={1} />
        <TreeFolder label="Applications" depth={2} />
        <TreeFolder label="LibreNMS" depth={3} />
        <TreeNote label="00-Overview.md" active depth={4} />
        <TreeNote label="01-Technical-Details.md" depth={4} />
        <TreeNote label="05-Monitoring.md" depth={4} />
        <TreeNote label="10-RACI.md" depth={4} />
        <TreeFolder label="Runbooks" depth={0} />
        <TreeNote label="Deploy-local.md" depth={1} />
        <TreeNote label="Restart-service.md" depth={1} />
        <TreeFolder label="Planning" depth={0} />
        <TreeNote label="MVP Roadmap.md" depth={1} />
      </nav>
      <section className="tag-section">
        <div className="section-title">Tags</div>
        <div className="tags">
          {["application 4", "la4 3", "runbook 2", "raci 1", "ops 3"].map((tag, index) => (
            <button key={tag} className={index === 0 ? "active" : ""}>{tag}</button>
          ))}
        </div>
      </section>
      <section className="trash-card">
        <span>Trash</span>
        <strong>3 items</strong>
      </section>
      <footer className="sidebar-footer">
        <span>Settings</span>
        <span>v0.visual-spike</span>
      </footer>
    </aside>
  );
}

function TreeFolder({ label, depth }: { label: string; depth: number }) {
  return <div className="tree-row folder" style={{ paddingLeft: 8 + depth * 14 }}>▾ <span>{label}</span></div>;
}

function TreeNote({ label, depth, active = false }: { label: string; depth: number; active?: boolean }) {
  return <div className={`tree-row note ${active ? "active" : ""}`} style={{ paddingLeft: 8 + depth * 14 }}>MD <span>{label}</span></div>;
}

function DocumentTopBar(props: {
  mode: ViewMode;
  setMode: (mode: ViewMode) => void;
  splitPreset: SplitPreset;
  setSplitPreset: (preset: SplitPreset) => void;
}) {
  return (
    <header className="document-top">
      <div className="tabs">
        {tabs.map((tab, index) => (
          <button key={tab} className={`tab ${index === 0 ? "active" : ""}`} title={`IBX/LA4/Applications/LibreNMS/${tab}`}>
            <span>{tab}</span>
            <b>×</b>
          </button>
        ))}
        <button className="new-tab">+</button>
      </div>
      <div className="doc-actions">
        {(["editor", "preview", "split"] as ViewMode[]).map((nextMode) => (
          <button key={nextMode} onClick={() => props.setMode(nextMode)} className={props.mode === nextMode ? "active" : ""}>
            {capitalize(nextMode)}
          </button>
        ))}
        {props.mode === "split" && (
          <div className="split-presets">
            {(["50/50", "60/40", "70/30"] as SplitPreset[]).map((preset) => (
              <button key={preset} onClick={() => props.setSplitPreset(preset)} className={props.splitPreset === preset ? "active" : ""}>{preset}</button>
            ))}
          </div>
        )}
        <button className="save-button">Save</button>
      </div>
    </header>
  );
}

function MarkdownToolbar() {
  return (
    <div className="markdown-toolbar">
      {["B", "I", "H1", "H2", "H3", "List", "Check", "Link", "Code", "Quote"].map((item) => (
        <button key={item}>{item}</button>
      ))}
    </div>
  );
}

function EditorPanel() {
  return (
    <section className="editor-panel">
      {lines.map((line, index) => (
        <div className="editor-line" key={`${index}-${line}`}>
          <span className="line-number">{index + 1}</span>
          <code>{line || " "}</code>
        </div>
      ))}
    </section>
  );
}

function PreviewPanel() {
  return (
    <article className="preview-panel">
      <h1>Application Documentation Pack</h1>
      <p>
        Welcome to the <strong>Application Documentation Pack</strong>.
        This pack contains everything you need to understand, run, operate, and extend the application.
      </p>
      <h2>What's included</h2>
      <ul className="checklist">
        <li className="done">System overview</li>
        <li className="done">Architecture diagram</li>
        <li>Configuration reference</li>
        <li>Operational runbooks</li>
      </ul>
      <h2>Overview</h2>
      <p>
        The application is built with <strong>Node.js</strong>, <strong>TypeScript</strong>, and <strong>PostgreSQL</strong>.
      </p>
      <blockquote>Keep this document up to date as the system evolves.</blockquote>
      <pre><code>systemctl restart librenms-worker</code></pre>
      <p className="link-line">Related: <a href="#">01-Technical-Details.md</a></p>
    </article>
  );
}

function RightPanel() {
  return (
    <aside className="right-panel">
      <div className="panel-tabs">
        <button className="active">Info</button>
        <button>Links</button>
      </div>
      <section className="info-list">
        <Info label="Title" value="Application Documentation Pack" />
        <Info label="Type" value="application" />
        <Info label="Status" value="Active" />
        <Info label="Tags" value="application, la4, ops" />
        <Info label="Created" value="2026-06-03" />
        <Info label="Updated" value="2026-06-03 20:58" />
        <Info label="Path" value="IBX/LA4/Applications/LibreNMS/00-Overview.md" />
      </section>
      <section className="links-panel">
        <div className="section-title">Internal Links</div>
        <div className="link-card resolved">01-Technical-Details.md <span>Resolved</span></div>
        <div className="link-card resolved">05-Monitoring.md <span>Resolved</span></div>
        <div className="link-card broken">99-Unknown.md <span>Broken</span></div>
      </section>
    </aside>
  );
}

function Info({ label, value }: { label: string; value: string }) {
  return (
    <div className="info-row">
      <span>{label}</span>
      <strong>{value}</strong>
    </div>
  );
}

function StatusBar() {
  return (
    <footer className="status-bar">
      <span className="saved-dot" />
      <span>All changes saved locally</span>
      <span>IBX/LA4/Applications/LibreNMS/00-Overview.md</span>
      <span className="right">Markdown · Ln 22, Col 1</span>
    </footer>
  );
}

function capitalize(value: string) {
  return value[0].toUpperCase() + value.slice(1);
}

createRoot(document.getElementById("root")!).render(<App />);

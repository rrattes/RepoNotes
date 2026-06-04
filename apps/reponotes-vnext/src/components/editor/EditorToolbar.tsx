const toolbarItems = ["H1", "H2", "H3", "B", "I", "code", "link", "list", "checklist", "quote", "table"];

export default function EditorToolbar() {
  return (
    <div className="editor-toolbar" aria-label="visual markdown tools">
      {toolbarItems.map((item) => (
        <button key={item} type="button">
          {item}
        </button>
      ))}
    </div>
  );
}

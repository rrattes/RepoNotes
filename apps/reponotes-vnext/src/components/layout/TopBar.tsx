export default function TopBar() {
  return (
    <header className="top-bar">
      <div className="brand">
        <span className="brand-name">RepoNotes vNext</span>
      </div>
      <div className="command-box" role="search">
        <span>Abrir comando...</span>
        <kbd>Ctrl K</kbd>
      </div>
      <div className="topbar-actions" aria-label="workspace actions">
        <span className="workspace-pill">infra-docs</span>
        <span className="security-pill">Private workspace</span>
        <button type="button" className="user-chip" aria-label="User and settings">
          RA
        </button>
      </div>
    </header>
  );
}

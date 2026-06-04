import type { MockNote } from "../../types/reponotes";
import EditorWorkspace from "./EditorWorkspace";
import InfoPanel from "./InfoPanel";
import LeftRail from "./LeftRail";
import RepositorySidebar from "./RepositorySidebar";
import StatusBar from "./StatusBar";
import TopBar from "./TopBar";

type AppShellProps = {
  activeNote: MockNote;
  activeNoteId: string;
  isInfoPanelOpen: boolean;
  onSelectNote: (noteId: string) => void;
  onToggleInfoPanel: () => void;
};

export default function AppShell({
  activeNote,
  activeNoteId,
  isInfoPanelOpen,
  onSelectNote,
  onToggleInfoPanel
}: AppShellProps) {
  return (
    <main className={`app-shell ${isInfoPanelOpen ? "info-open" : "info-closed"}`}>
      <TopBar />
      <section className="workspace-grid">
        <LeftRail />
        <RepositorySidebar activeNoteId={activeNoteId} onSelectNote={onSelectNote} />
        <EditorWorkspace
          activeNote={activeNote}
          activeNoteId={activeNoteId}
          isInfoPanelOpen={isInfoPanelOpen}
          onSelectNote={onSelectNote}
          onToggleInfoPanel={onToggleInfoPanel}
        />
        <InfoPanel isOpen={isInfoPanelOpen} note={activeNote} onToggle={onToggleInfoPanel} />
      </section>
      <StatusBar />
    </main>
  );
}

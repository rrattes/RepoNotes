import { useState } from "react";

import type { AutosaveStatus, MockNote } from "../../types/reponotes";
import type { RailItemId } from "../../types/reponotes";
import type { ServiceConnectionStatus } from "../../services/serviceRegistry";
import EditorWorkspace from "./EditorWorkspace";
import InfoPanel from "./InfoPanel";
import LeftRail from "./LeftRail";
import RepositorySidebar from "./RepositorySidebar";
import StatusBar from "./StatusBar";
import TopBar from "./TopBar";

type AppShellProps = {
  activeNote: MockNote;
  activeNoteId: string;
  autosaveStatus: AutosaveStatus;
  isInfoPanelOpen: boolean;
  onAutosaveStatusChange: (status: AutosaveStatus) => void;
  onSelectNote: (noteId: string) => void;
  onToggleInfoPanel: () => void;
  serviceConnectionStatus: ServiceConnectionStatus;
};

export default function AppShell({
  activeNote,
  activeNoteId,
  autosaveStatus,
  isInfoPanelOpen,
  onAutosaveStatusChange,
  onSelectNote,
  onToggleInfoPanel,
  serviceConnectionStatus
}: AppShellProps) {
  const [activeRailItem, setActiveRailItem] = useState<RailItemId>("files");

  return (
    <main className={`app-shell ${isInfoPanelOpen ? "info-open" : "info-closed"}`}>
      <TopBar />
      <section className="workspace-grid">
        <LeftRail activeRailItem={activeRailItem} onSelectRailItem={setActiveRailItem} />
        <RepositorySidebar activeNoteId={activeNoteId} activeRailItem={activeRailItem} onSelectNote={onSelectNote} />
        <EditorWorkspace
          activeNote={activeNote}
          activeNoteId={activeNoteId}
          isInfoPanelOpen={isInfoPanelOpen}
          onAutosaveStatusChange={onAutosaveStatusChange}
          onSelectNote={onSelectNote}
          onToggleInfoPanel={onToggleInfoPanel}
        />
        <InfoPanel isOpen={isInfoPanelOpen} note={activeNote} onToggle={onToggleInfoPanel} />
      </section>
      <StatusBar autosaveStatus={autosaveStatus} serviceConnectionStatus={serviceConnectionStatus} />
    </main>
  );
}

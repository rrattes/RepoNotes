import { useState } from "react";
import AppShell from "../components/layout/AppShell";
import { noteTabs, notesById } from "../data/mockRepository";

export default function App() {
  const [activeNoteId, setActiveNoteId] = useState(noteTabs[0].id);
  const [isInfoPanelOpen, setIsInfoPanelOpen] = useState(false);
  const activeNote = notesById[activeNoteId] ?? notesById.overview;

  return (
    <AppShell
      activeNote={activeNote}
      activeNoteId={activeNoteId}
      isInfoPanelOpen={isInfoPanelOpen}
      onSelectNote={setActiveNoteId}
      onToggleInfoPanel={() => setIsInfoPanelOpen((current) => !current)}
    />
  );
}

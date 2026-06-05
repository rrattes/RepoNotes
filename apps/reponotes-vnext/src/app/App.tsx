import { useEffect, useState } from "react";
import AppShell from "../components/layout/AppShell";
import { noteTabs, notesById } from "../data/mockRepository";
import { repositoryService } from "../services/serviceRegistry";
import type { AutosaveStatus, MockNote } from "../types/reponotes";

export default function App() {
  const [activeNoteId, setActiveNoteId] = useState(noteTabs[0].id);
  const [activeNote, setActiveNote] = useState<MockNote>(notesById[activeNoteId] ?? notesById.overview);
  const [autosaveStatus, setAutosaveStatus] = useState<AutosaveStatus>("saved");
  const [isInfoPanelOpen, setIsInfoPanelOpen] = useState(false);

  useEffect(() => {
    let isCurrent = true;

    repositoryService.getNoteById(activeNoteId).then((note) => {
      if (isCurrent) {
        setActiveNote(note ?? notesById.overview);
      }
    });

    return () => {
      isCurrent = false;
    };
  }, [activeNoteId]);

  return (
    <AppShell
      activeNote={activeNote}
      activeNoteId={activeNoteId}
      autosaveStatus={autosaveStatus}
      isInfoPanelOpen={isInfoPanelOpen}
      onAutosaveStatusChange={setAutosaveStatus}
      onSelectNote={setActiveNoteId}
      onToggleInfoPanel={() => setIsInfoPanelOpen((current) => !current)}
    />
  );
}

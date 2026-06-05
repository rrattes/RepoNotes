import { useEffect, useState } from "react";
import AppShell from "../components/layout/AppShell";
import {
  getServiceConnectionStatus,
  repositoryService,
  storageService,
  subscribeServiceConnectionStatus,
  type ServiceConnectionStatus
} from "../services/serviceRegistry";
import type { AutosaveStatus, MockNote } from "../types/reponotes";

export default function App() {
  const [activeNoteId, setActiveNoteId] = useState("");
  const [activeNote, setActiveNote] = useState<MockNote | null>(null);
  const [notes, setNotes] = useState<MockNote[]>([]);
  const [autosaveStatus, setAutosaveStatus] = useState<AutosaveStatus>("saved");
  const [serviceConnectionStatus, setServiceConnectionStatus] = useState<ServiceConnectionStatus>(
    getServiceConnectionStatus
  );
  const [isInfoPanelOpen, setIsInfoPanelOpen] = useState(false);

  useEffect(() => subscribeServiceConnectionStatus(setServiceConnectionStatus), []);

  useEffect(() => {
    let isCurrent = true;

    repositoryService.listNotes().then((loadedNotes) => {
      if (isCurrent) {
        setNotes(loadedNotes);

        if (!activeNoteId && loadedNotes[0]) {
          setActiveNoteId(loadedNotes[0].id);
        }

        if (activeNoteId && !loadedNotes.some((note) => note.id === activeNoteId)) {
          setActiveNoteId(loadedNotes[0]?.id ?? "");
        }
      }
    });

    return () => {
      isCurrent = false;
    };
  }, [activeNoteId]);

  useEffect(() => {
    let isCurrent = true;

    if (!activeNoteId) {
      setActiveNote(null);
      return () => {
        isCurrent = false;
      };
    }

    repositoryService.getNoteById(activeNoteId).then((note) => {
      if (isCurrent) {
        setActiveNote(note ?? null);
      }
    });

    return () => {
      isCurrent = false;
    };
  }, [activeNoteId]);

  async function refreshNotes(nextActiveNoteId?: string) {
    const loadedNotes = await repositoryService.listNotes();
    setNotes(loadedNotes);

    const selectedNote = nextActiveNoteId
      ? loadedNotes.find((note) => note.id === nextActiveNoteId)
      : loadedNotes[0];

    setActiveNoteId(selectedNote?.id ?? "");
    setActiveNote(selectedNote ?? null);
  }

  async function handleCreateNote() {
    const note = await storageService.createNote({
      bodyMarkdown: "# Untitled note\n",
      status: "Draft",
      title: "Untitled note",
      type: "note"
    });

    await refreshNotes(note.id);
  }

  async function handleMoveActiveNoteToTrash() {
    if (!activeNote) {
      return;
    }

    await storageService.moveNoteToTrash(activeNote.id);

    const loadedNotes = await repositoryService.listNotes();
    const nextNote = loadedNotes[0] ?? null;

    setNotes(loadedNotes);
    setActiveNoteId(nextNote?.id ?? "");
    setActiveNote(nextNote);
  }

  return (
    <AppShell
      activeNote={activeNote}
      activeNoteId={activeNoteId}
      autosaveStatus={autosaveStatus}
      isInfoPanelOpen={isInfoPanelOpen}
      notes={notes}
      onAutosaveStatusChange={setAutosaveStatus}
      onCreateNote={handleCreateNote}
      onMoveActiveNoteToTrash={handleMoveActiveNoteToTrash}
      onSelectNote={setActiveNoteId}
      onToggleInfoPanel={() => setIsInfoPanelOpen((current) => !current)}
      serviceConnectionStatus={serviceConnectionStatus}
    />
  );
}

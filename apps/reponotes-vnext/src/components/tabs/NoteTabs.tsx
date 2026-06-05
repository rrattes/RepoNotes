import type { MockNote } from "../../types/reponotes";

type NoteTabsProps = {
  activeNoteId: string;
  notes: MockNote[];
  onSelectNote: (noteId: string) => void;
};

export default function NoteTabs({ activeNoteId, notes, onSelectNote }: NoteTabsProps) {
  return (
    <div className="note-tabs" role="tablist" aria-label="open notes">
      {notes.map((tab) => (
        <button
          key={tab.id}
          className={`note-tab ${tab.id === activeNoteId ? "active" : ""}`}
          onClick={() => onSelectNote(tab.id)}
          title={tab.path}
          type="button"
        >
          <span>{tab.title}</span>
          <b aria-hidden="true">×</b>
        </button>
      ))}
    </div>
  );
}

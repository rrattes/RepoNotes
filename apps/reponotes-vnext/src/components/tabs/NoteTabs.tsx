import { noteTabs } from "../../data/mockRepository";

type NoteTabsProps = {
  activeNoteId: string;
  onSelectNote: (noteId: string) => void;
};

export default function NoteTabs({ activeNoteId, onSelectNote }: NoteTabsProps) {
  return (
    <div className="note-tabs" role="tablist" aria-label="open notes">
      {noteTabs.map((tab) => (
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

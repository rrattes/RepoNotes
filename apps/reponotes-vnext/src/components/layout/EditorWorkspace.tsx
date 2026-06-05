import type { AutosaveStatus, MockNote } from "../../types/reponotes";
import EditorToolbar from "../editor/EditorToolbar";
import VisualMarkdownEditor from "../editor/VisualMarkdownEditor";
import NoteTabs from "../tabs/NoteTabs";

type EditorWorkspaceProps = {
  activeNote: MockNote | null;
  activeNoteId: string;
  isInfoPanelOpen: boolean;
  notes: MockNote[];
  onAutosaveStatusChange: (status: AutosaveStatus) => void;
  onMoveActiveNoteToTrash: () => void;
  onSelectNote: (noteId: string) => void;
  onToggleInfoPanel: () => void;
};

export default function EditorWorkspace({
  activeNote,
  activeNoteId,
  isInfoPanelOpen,
  notes,
  onAutosaveStatusChange,
  onMoveActiveNoteToTrash,
  onSelectNote,
  onToggleInfoPanel
}: EditorWorkspaceProps) {
  return (
    <section className="editor-workspace">
      <div className="tabs-row">
        <NoteTabs activeNoteId={activeNoteId} notes={notes} onSelectNote={onSelectNote} />
        <button className="info-toggle" onClick={onToggleInfoPanel} type="button">
          {isInfoPanelOpen ? "Close Info" : "Info"}
        </button>
        <button
          className="note-trash-button"
          disabled={!activeNote}
          onClick={onMoveActiveNoteToTrash}
          type="button"
        >
          Move to trash
        </button>
      </div>
      {activeNote ? (
        <>
          <EditorToolbar />
          <VisualMarkdownEditor note={activeNote} onAutosaveStatusChange={onAutosaveStatusChange} />
        </>
      ) : (
        <section className="empty-editor-state">
          <strong>No note selected</strong>
          <span>Create a note or select one from the repository.</span>
        </section>
      )}
    </section>
  );
}

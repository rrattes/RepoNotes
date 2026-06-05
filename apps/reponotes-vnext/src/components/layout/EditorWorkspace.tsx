import type { AutosaveStatus, MockNote } from "../../types/reponotes";
import EditorToolbar from "../editor/EditorToolbar";
import VisualMarkdownEditor from "../editor/VisualMarkdownEditor";
import NoteTabs from "../tabs/NoteTabs";

type EditorWorkspaceProps = {
  activeNote: MockNote;
  activeNoteId: string;
  isInfoPanelOpen: boolean;
  onAutosaveStatusChange: (status: AutosaveStatus) => void;
  onSelectNote: (noteId: string) => void;
  onToggleInfoPanel: () => void;
};

export default function EditorWorkspace({
  activeNote,
  activeNoteId,
  isInfoPanelOpen,
  onAutosaveStatusChange,
  onSelectNote,
  onToggleInfoPanel
}: EditorWorkspaceProps) {
  return (
    <section className="editor-workspace">
      <div className="tabs-row">
        <NoteTabs activeNoteId={activeNoteId} onSelectNote={onSelectNote} />
        <button className="new-tab-button" type="button" title="Nova aba visual">
          +
        </button>
        <button className="info-toggle" onClick={onToggleInfoPanel} type="button">
          {isInfoPanelOpen ? "Close Info" : "Info"}
        </button>
      </div>
      <EditorToolbar />
      <VisualMarkdownEditor note={activeNote} onAutosaveStatusChange={onAutosaveStatusChange} />
    </section>
  );
}

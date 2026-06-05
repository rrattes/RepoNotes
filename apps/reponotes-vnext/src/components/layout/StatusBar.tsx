import type { AutosaveStatus } from "../../types/reponotes";

const autosaveStatusText: Record<AutosaveStatus, string> = {
  changed: "Unsaved changes",
  error: "Save error",
  saved: "All changes saved locally",
  saving: "Saving..."
};

type StatusBarProps = {
  autosaveStatus: AutosaveStatus;
};

export default function StatusBar({ autosaveStatus }: StatusBarProps) {
  return (
    <footer className="status-bar">
      <span className="saved-indicator" />
      <span>{autosaveStatusText[autosaveStatus]}</span>
      <span>infra-docs / main</span>
      <span className="status-spacer" />
      <span>842 words</span>
      <span>100%</span>
    </footer>
  );
}

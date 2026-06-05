import type { AutosaveStatus } from "../../types/reponotes";
import type { ServiceConnectionStatus } from "../../services/serviceRegistry";

const autosaveStatusText: Record<AutosaveStatus, string> = {
  changed: "Unsaved changes",
  error: "Save error",
  saved: "All changes saved locally",
  saving: "Saving..."
};

const serviceConnectionStatusText: Record<ServiceConnectionStatus, string> = {
  connected: "API connected",
  mock: "Local mock",
  "offline-fallback": "API offline, using local mock"
};

type StatusBarProps = {
  autosaveStatus: AutosaveStatus;
  serviceConnectionStatus: ServiceConnectionStatus;
};

export default function StatusBar({ autosaveStatus, serviceConnectionStatus }: StatusBarProps) {
  return (
    <footer className="status-bar">
      <span className="saved-indicator" />
      <span>{autosaveStatusText[autosaveStatus]}</span>
      <span>{serviceConnectionStatusText[serviceConnectionStatus]}</span>
      <span>infra-docs / main</span>
      <span className="status-spacer" />
      <span>842 words</span>
      <span>100%</span>
    </footer>
  );
}

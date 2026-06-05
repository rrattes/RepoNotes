import type { MockNote } from "../../types/reponotes";

type InfoPanelProps = {
  isOpen: boolean;
  note: MockNote | null;
  onToggle: () => void;
};

export default function InfoPanel({ isOpen, note, onToggle }: InfoPanelProps) {
  return (
    <aside className={`info-panel ${isOpen ? "open" : "closed"}`}>
      {!isOpen ? (
        <button className="info-rail-button" onClick={onToggle} type="button" title="Abrir Info">
          Info
        </button>
      ) : (
        <>
          <header className="info-header">
            <div>
              <span className="label">Panel</span>
              <strong>Info</strong>
            </div>
            <button onClick={onToggle} type="button">
              Close
            </button>
          </header>

          <section className="info-section">
            <h3>Metadata</h3>
            {note ? (
              <>
                <InfoRow label="Title" value={note.title} />
                <InfoRow label="Type" value={note.type} />
                <InfoRow label="Status" value={note.status} />
                <InfoRow label="Owner" value={note.owner} />
                <InfoRow label="Reviewer" value={note.reviewer} />
                <InfoRow label="Version" value={note.version} />
                <InfoRow label="Created" value={note.created} />
                <InfoRow label="Updated" value={note.updated} />
                <InfoRow label="Path" value={note.path} />
              </>
            ) : (
              <p className="muted-copy">No note selected.</p>
            )}
          </section>

          {note ? (
            <>
              <section className="info-section">
                <h3>Tags</h3>
                <div className="tag-chips">
                  {note.tags.map((tag) => (
                    <span key={tag}>{tag}</span>
                  ))}
                </div>
              </section>

              <section className="info-section">
                <h3>Backlinks</h3>
                <div className="backlinks">
                  {note.backlinks.map((link) => (
                    <span key={link}>{link}</span>
                  ))}
                </div>
              </section>
            </>
          ) : null}

          <section className="info-actions">
            <button type="button">Duplicate</button>
            <button className="danger" disabled={!note} type="button">
              Move to Trash
            </button>
          </section>
        </>
      )}
    </aside>
  );
}

function InfoRow({ label, value }: { label: string; value: string }) {
  return (
    <div className="info-row">
      <span>{label}</span>
      <strong>{value}</strong>
    </div>
  );
}

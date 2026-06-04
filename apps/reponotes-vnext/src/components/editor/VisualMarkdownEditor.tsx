import type { MockNote } from "../../types/reponotes";

type VisualMarkdownEditorProps = {
  note: MockNote;
};

export default function VisualMarkdownEditor({ note }: VisualMarkdownEditorProps) {
  return (
    <article className="visual-editor" aria-label="visual markdown editor mock">
      <header className="document-header">
        <div className="document-badges">
          <span>{note.type}</span>
          <span>{note.status}</span>
          <span>owner: {note.owner}</span>
        </div>
        <h1>{note.title}</h1>
      </header>

      <section className="callout">
        <strong>Operational documentation pack</strong>
        <p>
          Este pacote organiza a documentacao tecnica operacional de uma aplicacao, incluindo visao geral,
          arquitetura, operacao, monitoramento, RACI e runbooks.
        </p>
      </section>

      <section className="document-section">
        <h2>1. Visao Geral</h2>
        <p>
          O objetivo deste documento e manter uma fonte local, exportavel e revisavel sobre a aplicacao, seu
          ambiente, seus donos e os processos criticos de operacao.
        </p>
      </section>

      <section className="document-section">
        <h2>2. Estrutura do Pack</h2>
        <table>
          <thead>
            <tr>
              <th>Documento</th>
              <th>Objetivo</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {note.table.map((row) => (
              <tr key={`${row.document}-${row.status}`}>
                <td>{row.document}</td>
                <td>{row.objective}</td>
                <td>
                  <span className={`status-chip ${row.status.toLowerCase()}`}>{row.status}</span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>

      <section className="document-section two-column">
        <div>
          <h2>3. Convencoes e Padroes</h2>
          <p>
            Use nomes consistentes, links internos para runbooks e metadados suficientes para governanca,
            revisao e exportacao.
          </p>
          <pre><code>repo/applications/librenms/00-overview.md</code></pre>
        </div>
        <div>
          <h2>4. Checklist de Prontidao</h2>
          <ul className="readiness-list">
            <li className="checked">Owner definido</li>
            <li className="checked">Monitoramento documentado</li>
            <li>Runbook critico criado</li>
            <li>RACI revisada</li>
          </ul>
        </div>
      </section>
    </article>
  );
}

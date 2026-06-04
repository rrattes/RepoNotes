# RepoNotes vNext

RepoNotes vNext is the clean React/Vite/TypeScript foundation for the next main version of RepoNotes. It is a local-first operational documentation editor direction, planned for a future Tauri desktop shell.

This app does not reuse or promote the old visual spike directly. It is a new product shell with mock data and no real filesystem integration.

## Stack

- React
- Vite
- TypeScript
- Modern CSS
- Mocked in-memory repository data
- Milkdown/Crepe visual Markdown editor
- lucide-react icons for the Activity Bar / Left Rail

Tauri is intentionally not initialized in this round.

## How To Run

```powershell
cd apps/reponotes-vnext
npm install
npm run dev -- --port 5174
```

Open:

```text
http://127.0.0.1:5174/
```

## Implemented In This Round

- Clean vNext folder structure.
- Dark premium desktop-style shell.
- Top bar with RepoNotes vNext identity, command/search box, and mocked window controls.
- Left Activity Bar with real lucide icons, active state, disabled future items, and lower utility actions.
- Repository navigation sidebar with mock tree and visible trash summary.
- Compact note tabs with active-tab state.
- Visual Markdown editor surface backed by Milkdown/Crepe.
- Compact editor toolbar.
- Info panel closed by default with local open/close state.
- Status bar with mocked autosave state.

## Visual Markdown Editor

The current editor surface uses `@milkdown/crepe` as a controlled Visual Markdown editing spike. It mounts a real ProseMirror/Milkdown editor with initial Markdown content in memory.

The main UI is intentionally product-like: the visible center area shows the current document title and the visual editor, not spike/debug labels. Generated Markdown remains available internally through editor state and Crepe APIs for future autosave/export work, but it is not shown in the primary user experience.

This validates that:

- A visual editor can load clean Markdown.
- Headings, blockquotes, tables, checklists, and fenced code blocks can be displayed as editable visual content.
- Markdown can be read back from the editor in memory.

Current limitations:

- No filesystem integration.
- No autosave.
- No frontmatter boundary handling.
- No Tauri shell.
- No protected notes.
- No visible Markdown debug panel by default.
- The production build passes, but Crepe currently adds a large JavaScript chunk. This must be measured before promoting the spike to product architecture.

## Mocked

- Repository data.
- Tree navigation.
- Tabs and selected note content.
- Activity Bar sections beyond Repository/Search/Trash/Settings.
- Visual Markdown editor persistence.
- Autosave status.
- Window controls.
- Info panel metadata.
- Backlinks and actions.

## Not Implemented Yet

- Final Visual Markdown editor architecture.
- Tauri shell.
- Filesystem access.
- Persistence.
- Autosave logic.
- Real command palette.
- Search execution.
- Real note creation, rename, delete, restore, or export.
- Password protected notes.

## Next Steps

1. Validate Milkdown/Crepe Markdown round-trip quality with frontmatter and larger technical notes.
2. Tauri shell.
3. Filesystem MVP.

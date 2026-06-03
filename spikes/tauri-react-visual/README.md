# RepoNotes Tauri + React Visual Spike

## Objective

This spike evaluates whether a web-based desktop UI stack could make the RepoNotes interface easier to evolve, denser, and more responsive. It is intentionally visual-only.

This does not migrate the Avalonia app and does not touch real RepoNotes storage, filesystem logic, persistence, or backend behavior.

## How To Run

```powershell
cd spikes/tauri-react-visual
npm install
npm run dev
```

Open the local URL printed by Vite. In this validation round it used:

```text
http://127.0.0.1:5173/
```

## Stack Used

- React
- TypeScript
- Vite
- Modern CSS
- Mocked in-memory data

Tauri is not wired yet. The project is structured as a fast React/Vite visual prototype that can be wrapped by Tauri in a later spike if the UI direction proves useful.

## Visual Scope Implemented

- Dark desktop-style shell.
- Compact top window bar with simulated window controls.
- Left repository sidebar with repository context, search, create actions, explorer tree, tags, trash summary, and settings footer.
- Compact note tabs with active state, close buttons, and new-tab affordance.
- Editor / Preview / Split mode switch.
- Split presets: `50/50`, `60/40`, and `70/30`.
- Markdown toolbar with common formatting actions.
- VS Code/Obsidian-style Markdown editor surface with line numbers.
- Rendered visual preview using mocked rich content.
- Right info panel with note metadata and internal links.
- Visual states for selected note, active tab, active tag, active mode, save status, and trash count.

## Mocked Data

All data is hardcoded in memory:

- Repository: `infra-docs`
- Explorer tree for IBX/LA4/Applications/LibreNMS, Runbooks, and Planning.
- Open tabs for `00-Overview.md`, `01-Technical-Details.md`, and `10-RACI.md`.
- Tags, trash count, metadata, internal links, editor text, and preview content.

## Not Implemented

- Real Tauri shell.
- Filesystem access.
- RepoNotes storage integration.
- Persistence.
- Real Markdown parsing.
- Real tabs/documents.
- Search/filter execution.
- Save behavior.
- Window chrome behavior.
- Backend commands.

## Preliminary Comparison With Avalonia

React/Vite makes high-density layout iteration very fast. CSS grid/flex, responsive constraints, stateful visual variants, and interactive prototypes are easier to adjust than equivalent XAML styling. The spike suggests React could help with rapid UI exploration, especially for tabs, split view, responsive side panels, and information-dense tool surfaces.

Avalonia remains the actual product stack. It already owns the real MVVM, filesystem, tests, native preview pipeline, and Windows desktop behavior. A Tauri migration would add packaging/runtime questions and a new bridge layer for filesystem/storage operations.

## Preliminary Recommendation

Use this spike as a visual exploration tool, not as a migration decision. React/Tauri appears promising for UI velocity and responsive desktop layout experiments, but RepoNotes should stay on Avalonia until a separate technical spike proves:

- Tauri packaging is clean on Windows.
- Local-first filesystem workflows remain reliable.
- Native window behavior is acceptable.
- Test strategy remains strong.
- Markdown editor/preview behavior can match or exceed the Avalonia MVP.

## Suggested Next Tests

- Wrap the Vite prototype with Tauri and validate Windows packaging.
- Prototype real resize/collapse behavior with persisted layout settings.
- Compare memory footprint and startup time against the Avalonia app.
- Test keyboard shortcuts and command palette responsiveness in the web stack.
- Validate a minimal filesystem read-only bridge before considering any migration discussion.

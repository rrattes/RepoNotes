# RepoNotes UI Guide

## Visual Direction

RepoNotes should feel like a premium dark desktop productivity app, closer to Obsidian or VS Code in density and workflow focus than to a marketing website or generic form UI.

## vNext Definitive UI Direction

RepoNotes vNext uses the approved dark premium reference image as the definitive visual direction. The vNext UI should be rebuilt in React/Vite/TypeScript first, with Tauri added later, and should not be copied from the old Avalonia layout or the first visual spike without reconsidering each element.

Core vNext rules:

- Visual Markdown Editor-first: the main editor is the product surface.
- The user can type Markdown and see visual formatting while editing.
- Clean Markdown remains the saved/exportable source format.
- No separate Preview mode in the initial vNext MVP.
- No Split mode as the main initial MVP flow.
- No primary Save button; autosave with debounce is the default.
- The note title appears primarily in the tab.
- Do not duplicate the note title as a large heading above the editor.
- Tags live in the Info panel.
- The Info panel is closed by default.
- The left sidebar is for navigation only.
- A left rail with icons provides compact access to workspace areas.
- Trash remains visible and clear, but not dominant.
- The writing area gets maximum horizontal and vertical space.
- Every visible button must perform a real action. If it does not work yet, remove it instead of showing a placeholder.
- The old Avalonia app is a functional reference/legacy MVP; it is not the vNext UI source.
- The old React visual spike is a reference artifact only; it is not the vNext product base.

## Layout

- Main window uses a three-row grid:
  - Integrated custom window bar.
  - Main content.
  - Status bar.
- Main content uses a three-column grid:
  - Compact sidebar.
  - Flexible editor-first center column.
  - Compact preview/info panel.
- The left repository sidebar and the right info/context panel can be collapsed and expanded to improve usable writing space on smaller screens.
- Collapsed side panels keep a narrow rail with an explicit re-open control so the state is never irreversible.
- RepoNotes uses client-side custom window chrome in Avalonia. The native Windows title bar must not appear as a duplicated separate bar.
- The custom window controls are real controls wired to the `Window`: minimize sets `WindowState.Minimized`, maximize/restore toggles `WindowState`, and close calls `Close()`.
- The window remains resizable and includes a compact draggable area in the integrated bar.

## Top Area

- The app has a compact integrated window bar for client-side chrome only; it is not a heavy global app top bar.
- Repository selection and search live in the top of the left sidebar, close to note navigation.
- The repository area in the sidebar presents the current repository context and may act as the local repository switcher when wired to `OpenRepositoryCommand`.
- The repository switcher tooltip should be `Abrir ou trocar repositorio local`.
- Note creation uses two sidebar actions: `Nova nota` remains the fast default path for a free Markdown note, while `Novo por template` uses a compact template picker in the sidebar.
- The template picker should stay visually secondary to repository navigation: compact combo box, short selected-template description, and one small action button.
- Global actions such as settings live in the lower-left sidebar toolbar.
- Lower-left global actions should use clear compact labels and tooltips. Avoid cryptic placeholder labels such as `N`, `*`, or `Cfg`.
- Sidebar item actions such as rename and delete may remain in the lower-left toolbar, but the primary explorer ergonomics should use context menus on notes and folders.
- Trash actions live in the lower-left sidebar area as a compact picker plus small action buttons for restore, permanent delete, and empty trash.
- Permanent delete controls must remain visually secondary and should gain explicit confirmation UX in a future round.
- The only persistent top area inside the main content is the compact document context bar above the editor.
- The document context bar shows real open note tabs, Editor/Preview/Split mode, split presets when applicable, and note-scoped actions.
- The note title belongs to the active tab as the primary note identity. Do not duplicate it as a large editable heading above the editor.
- The full note path is secondary information and should live in the tab tooltip, the right-side Info panel, and the status bar. Avoid showing a large breadcrumb in the central editor header.
- Note tabs are functional, not decorative: selecting a note opens a tab or activates the existing tab; duplicate tabs for the same note are not created.
- Note tabs should remain compact, around `30px` high, with trimmed titles and a restrained close button. Long titles must not push editor mode controls or save actions out of view.
- Active tabs use a stronger panel background and open tabs with unsaved edits show a compact `*` dirty indicator.
- Each tab has a small close action aligned inside the tab. Closing a dirty tab saves first; if saving fails, the tab stays open and the status shows the save error.
- Open note tabs expose a compact context menu for close, close others, close all, reveal in explorer, and copy path.
- Switching between tabs must preserve unsaved edits in each tab and must not auto-save just because focus changed.
- The central document area has a clear `Editor` / `Preview` / `Split` mode toggle. `Editor` shows the plain Markdown TextBox and formatting toolbar; `Preview` shows the rendered Markdown in the main workspace and hides the formatting toolbar; `Split` shows the Markdown editor and rendered preview side by side.
- The top editor row should use compact tabs on the left, flexible empty space in the middle, and fixed-width actions on the right. `Editor`, `Preview`, `Split`, split presets, and `Salvar` must remain aligned and must not overlap tabs.
- Disabled future actions should not pollute the editor top row. Remove inactive `Info`/`Tags` style actions until they have real behavior; formatting commands stay in the editor toolbar.
- The app must not draw fake window controls. When custom chrome is used, minimize, maximize/restore, close, drag, and resize behavior must be real and validated.
- The integrated window bar should stay around `34px` high, dark, quiet, and visually secondary to the editor.

## Metadata Panel

- Basic frontmatter metadata lives in the right-side Info area so it does not reduce editor width.
- Editable metadata should stay compact and stacked: `type`, `status`, and `tags`.
- Tags are edited as comma-separated text in the initial MVP UI.
- `created`, `updated`, and `path` are read-only informational fields.
- Metadata editing must mark the note as changed and rely on the normal save flow to write frontmatter.

## Tags Sidebar

- The sidebar `TAGS` section must show real tags read from note frontmatter, not mock chips.
- Tag chips are compact and show the tag name plus note count, for example `infra 3`.
- Clicking a tag filters the note tree to notes containing that tag; clicking the active tag again or using `Limpar` removes the filter.
- Tag filtering and text search should work together when both are active.
- The active tag needs a subtle visual indicator while preserving the dark compact sidebar rhythm.
- Notes inside `.reponotes-trash` must not contribute tags or counts.
- Future encrypted or locked content must not expose tag counts or searchable metadata while locked.

## Context Menus

- Explorer nodes expose a compact dark context menu with `Open`, `Open in Tab` for notes, `New Note`, `New Folder`, `Rename`, `Move to Trash`, and `Copy Path`.
- Right-clicking an explorer node should select that node before executing an action so rename, delete, and create-here actions use the expected target.
- Note tab context menus provide `Close`, `Close Others`, `Close All`, `Reveal in Explorer`, and `Copy Path`.
- Trash controls provide context actions for `Restore`, `Delete Permanently`, and `Empty Trash` while keeping `.reponotes-trash` out of the main tree/search.
- Context menus should use the same dark surface, subtle border, compact spacing, and clear labels as the rest of the app.
- Future rounds should add explicit confirmation UI for permanent trash deletion and empty-trash actions.

## Search Feedback

- Search lives in the top of the sidebar and stays compact.
- Text input uses a short debounce before filtering so typing does not recalculate the tree on every keypress.
- A small feedback line under the search box shows result count, `Buscando...`, or `Nenhum resultado encontrado`.
- A small clear action inside the search field removes the query; `Esc` may clear search while the field is focused.
- `Ctrl+K` should focus the search field.
- Search result highlighting should remain subtle; matched note rows may use a small accent indicator rather than heavy inline markup.
- Search and tag filters should combine without relisting `.reponotes-trash`.

## Internal Links

- Wiki-style links use `[[Nome da Nota]]`.
- Internal links are resolved against note title first, then Markdown file name.
- Resolved links should look actionable and use the accent system subtly.
- Broken links should remain visible with a quiet `Quebrado` state and must not crash the preview.
- The initial MVP may show detected links as a compact list in the preview/info panel instead of inline rich text inside paragraphs.
- Clicking a resolved internal link should open the target note when possible; broken links must not auto-create notes in this round.

## Rich Markdown Preview

- The preview renders Markdown through native Avalonia controls, not WebView.
- The primary rendered preview belongs in the central document area behind the `Preview` mode, not as the main content of the right sidebar.
- Split View belongs to the Markdown Power Editor path: it keeps Markdown source editing on the left and native rendered preview on the right.
- Split View must use the same preview block pipeline as Preview mode; do not duplicate Markdown rendering logic in XAML or ViewModel.
- In Split View, the formatting toolbar remains visible because the Markdown TextBox is still editable.
- Split View uses stable width presets instead of free drag in the MVP: `50/50`, `60/40`, and `70/30`.
- The center line in Split View is a visual separator only; it must not look like a draggable control when free drag is not reliable.
- Free drag resizing can be revisited later, but it must not be shown unless repeated adjustments are stable in 1366x768 and 1600x900.
- Split View does not yet synchronize scroll between editor and preview.
- Split View should remain compact enough for 1366x768, but the center editor/preview workspace takes priority over decorative chrome.
- Collapsing either side panel should immediately give the central Editor/Preview/Split workspace more horizontal room without changing the active tab or document mode.
- Markdown formatting commands should be available by toolbar and keyboard shortcut: `Ctrl+B`, `Ctrl+I`, `Ctrl+Alt+1`, `Ctrl+Alt+2`, `Ctrl+Alt+3`, `Ctrl+Shift+7`, `Ctrl+Shift+8`, `Ctrl+\``, `Ctrl+Shift+Q`, and contextual `Ctrl+K`.
- `Ctrl+K` is contextual: while a Markdown editor has focus it applies link formatting; outside the editor it focuses the sidebar search field.
- A compact Command Palette opens with `Ctrl+Shift+P` and provides keyboard access to common editor modes, Markdown formatting, insertions, and safe note actions.
- The Command Palette filters commands as the user types; `Enter` executes the selected command, `Esc` closes, and arrow keys move the selection.
- The Command Palette complements toolbar buttons and shortcuts; it must not replace them or become a plugin system in the MVP.
- The right sidebar focuses on note info, internal links, and metadata; it must not present a confusing fake `Preview` tab when preview mode is handled centrally.
- Paragraphs, headings, and list items use inline runs so Markdown markers are removed from the visual preview.
- `**bold**` must render with real bold weight.
- `*italic*` must render with real italic style.
- `***bold italic***` must combine bold and italic.
- Inline code uses a monospace font and accent color so it is visually distinct while staying compact in the dark preview.
- Markdown links render as visually distinct underlined link text with the link accent color; they do not need to be clickable in this preview layer unless wired by a separate command.
- Lists render with a visual bullet marker instead of exposing the Markdown `-` marker.
- Checklists render with visual checked/unchecked markers instead of exposing raw `- [ ]` or `- [x]` text.
- Code blocks, blockquotes, checklists, lists, and simple tables remain block-level preview elements.

## Future Visual Markdown Mode

- RepoNotes may add a future `Visual Mode` alongside the existing Markdown source editor.
- `Markdown Mode` remains the explicit source-editing mode and must not be removed.
- `Visual Mode` should present headings, emphasis, lists, checklists, quotes, inline code, code blocks, and links as editable visual content while saving back to Markdown.
- The UI should make mode switching clear without implying that Markdown files are converted to another source format.
- Visual Mode must work with functional note tabs and preserve dirty/saved state per tab.
- Frontmatter should not appear as rich editable document content; it stays in the Info panel and storage/frontmatter parser flow.
- WebView2/editor-web is acceptable only after a technical spike proves packaging, theme integration, Markdown round-trip, and MVVM/tab synchronization are practical.

## Simple Name Dialog

- File and folder creation/rename flows use a compact dark text prompt instead of relying only on automatic names.
- The prompt must show a clear title, a short message, one text field, and `Cancelar` / `Confirmar` actions.
- Empty names and Windows-invalid characters should be rejected in the dialog when possible; storage still remains responsible for safe file names and avoiding overwrites.
- If a prompt is unavailable, the app may fall back to the previous automatic name so file operations remain usable.

## Current Density Targets

- Sidebar: around 250px to 260px, currently `252px`.
- Preview panel: around 320px to 340px, currently `326px`.
- Collapsed side rail: currently `42px` for each side panel.
- Integrated window bar: compact, currently `34px`, used only for drag/window controls and very light context.
- Status bar: compact, around 38px to 42px, currently `38px`.
- Editor column gets priority for all extra horizontal space.
- Editor chrome should stay compact: document context bar around `34px`, no separate title row, and formatting toolbar around `34px` to `40px`.
- Editor outer padding should stay tight, roughly `10px` to `12px`, so the writing area feels like the main workspace rather than a small card.

## Theme

- App background: `#0B111A`
- Top bar: `#0A0F17`
- Sidebar: `#0D1520`
- Panels/cards: `#101823`
- Editor: `#0B1018`
- Border: `#223041`
- Divider: `#1F2937`
- Primary text: `#E5E7EB`
- Secondary text: `#9CA3AF`
- Weak text: `#6B7280`
- Accent: `#6366F1`
- Accent hover: `#4F46E5`
- Saved status: `#22C55E`

## UI Rules

- Avoid white backgrounds.
- Avoid default gray Windows-looking buttons.
- Keep the editor visually dominant.
- Prefer dense, legible controls over showcase-sized controls.
- Keep preview useful but visually secondary; it should not compete with the writing surface.
- Preview should render the current note Markdown through native dark Avalonia blocks rather than static mock text; code and simple tables may use compact monospace panels.
- Prefer compact toolbar groups with subtle dividers over large standalone buttons.
- Keep global actions out of a top bar; repository selection/search belong in the sidebar and settings belongs in the lower-left sidebar toolbar.
- Centralize reusable colors and control styling in `RepoNotes.App/Styles/AppTheme.axaml`.
- Keep visual-only mock elements clearly separated from business logic.

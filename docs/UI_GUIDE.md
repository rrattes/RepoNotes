# RepoNotes UI Guide

## Visual Direction

RepoNotes should feel like a premium dark desktop productivity app, closer to Obsidian or VS Code in density and workflow focus than to a marketing website or generic form UI.

## Layout

- Main window uses a three-row grid:
  - Integrated custom window bar.
  - Main content.
  - Status bar.
- Main content uses a three-column grid:
  - Compact sidebar.
  - Flexible editor-first center column.
  - Compact preview/info panel.
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
- Sidebar item actions such as rename and delete may live in the lower-left toolbar as compact controls while the app has no context menu; labels and tooltips must remain explicit.
- Trash actions live in the lower-left sidebar area as a compact picker plus small action buttons for restore, permanent delete, and empty trash.
- Permanent delete controls must remain visually secondary and should gain explicit confirmation UX in a future round.
- The only persistent top area inside the main content is the document context bar above the editor.
- The document context bar shows real open note tabs, breadcrumb, Editor/Preview mode, and note-scoped actions.
- Note tabs are functional, not decorative: selecting a note opens a tab or activates the existing tab; duplicate tabs for the same note are not created.
- Active tabs use a stronger panel background and open tabs with unsaved edits show a compact `*` dirty indicator.
- Each tab has a small close action. Closing a dirty tab saves first; if saving fails, the tab stays open and the status shows the save error.
- Switching between tabs must preserve unsaved edits in each tab and must not auto-save just because focus changed.
- Breadcrumbs should make the current context visible without becoming a large header, for example `sample-repository / Inbox\Bem-vindo.md`.
- The central document area has a clear `Editor` / `Preview` mode toggle. `Editor` shows the plain Markdown TextBox and formatting toolbar; `Preview` shows the rendered Markdown in the main workspace and hides the formatting toolbar.
- Context actions such as Save, Info, and Tags belong near the document tab/breadcrumb, while formatting commands stay in the editor toolbar.
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

## Simple Name Dialog

- File and folder creation/rename flows use a compact dark text prompt instead of relying only on automatic names.
- The prompt must show a clear title, a short message, one text field, and `Cancelar` / `Confirmar` actions.
- Empty names and Windows-invalid characters should be rejected in the dialog when possible; storage still remains responsible for safe file names and avoiding overwrites.
- If a prompt is unavailable, the app may fall back to the previous automatic name so file operations remain usable.

## Current Density Targets

- Sidebar: around 250px to 260px, currently `252px`.
- Preview panel: around 320px to 340px, currently `326px`.
- Integrated window bar: compact, currently `34px`, used only for drag/window controls and very light context.
- Status bar: compact, around 38px to 42px, currently `38px`.
- Editor column gets priority for all extra horizontal space.
- Editor chrome should stay compact: document context bar around `38px`, title row around `46px` to `50px`, and formatting toolbar around `34px` to `40px`.
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

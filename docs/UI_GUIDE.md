# RepoNotes UI Guide

## Visual Direction

RepoNotes should feel like a premium dark desktop productivity app, closer to Obsidian or VS Code in density and workflow focus than to a marketing website or generic form UI.

## Layout

- Main window uses a two-row grid:
  - Main content.
  - Status bar.
- Main content uses a three-column grid:
  - Compact sidebar.
  - Flexible editor-first center column.
  - Compact preview/info panel.
- The internal top bar has been removed. The MVP uses native Windows/Avalonia window controls and should avoid app-level chrome that repeats window or product branding.
- Custom window chrome can be evaluated in a future design round, but it is intentionally out of scope for now.

## Top Area

- The app no longer has an internal global top bar.
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
- The document context bar should show the active document tab, breadcrumb, and note-scoped actions.
- Breadcrumbs should make the current context visible without becoming a large header, for example `sample-repository / Inbox\Bem-vindo.md`.
- Context actions such as Save, Info, and Tags belong near the document tab/breadcrumb, while formatting commands stay in the editor toolbar.
- The app must not draw fake window controls in the top area; native OS/Avalonia window controls remain responsible for minimize, maximize, and close.

## Metadata Panel

- Basic frontmatter metadata lives in the right-side Info area so it does not reduce editor width.
- Editable metadata should stay compact and stacked: `type`, `status`, and `tags`.
- Tags are edited as comma-separated text in the initial MVP UI.
- `created`, `updated`, and `path` are read-only informational fields.
- Metadata editing must mark the note as changed and rely on the normal save flow to write frontmatter.

## Simple Name Dialog

- File and folder creation/rename flows use a compact dark text prompt instead of relying only on automatic names.
- The prompt must show a clear title, a short message, one text field, and `Cancelar` / `Confirmar` actions.
- Empty names and Windows-invalid characters should be rejected in the dialog when possible; storage still remains responsible for safe file names and avoiding overwrites.
- If a prompt is unavailable, the app may fall back to the previous automatic name so file operations remain usable.

## Current Density Targets

- Sidebar: around 250px to 260px, currently `252px`.
- Preview panel: around 320px to 340px, currently `326px`.
- Internal top bar: removed.
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

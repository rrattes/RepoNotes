# RepoNotes UI Guide

## Visual Direction

RepoNotes should feel like a premium dark desktop productivity app, closer to Obsidian or VS Code in density and workflow focus than to a marketing website or generic form UI.

## Layout

- Main window uses a three-row grid:
  - Top bar.
  - Main content.
  - Status bar.
- Main content uses a three-column grid:
  - Compact sidebar.
  - Flexible editor-first center column.
  - Compact preview/info panel.
- The MVP uses the native Windows/Avalonia window controls. The app top bar must not draw its own minimize, maximize, or close buttons.
- Custom window chrome can be evaluated in a future design round, but it is intentionally out of scope for now.

## Top Area

- The top area is split conceptually into global app navigation and document context.
- The global bar should stay compact and contain app-level controls: menu/logo, RepoNotes name, repository selector, search, theme, and settings.
- The document context bar should sit above the editor and show the active document tab, breadcrumb, and note-scoped actions.
- Breadcrumbs should make the current context visible without becoming a large header, for example `sample-repository / Inbox\Bem-vindo.md`.
- Context actions such as Save, Info, and Tags belong near the document tab/breadcrumb, while formatting commands stay in the editor toolbar.
- The app must not draw fake window controls in the top area; native OS/Avalonia window controls remain responsible for minimize, maximize, and close.

## Current Density Targets

- Sidebar: around 250px to 260px, currently `252px`.
- Preview panel: around 320px to 340px, currently `326px`.
- Top bar: around 54px to 58px, currently `54px`.
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
- Prefer compact toolbar groups with subtle dividers over large standalone buttons.
- Keep top bar actions limited to application controls such as repository selection, search, theme, and settings.
- Centralize reusable colors and control styling in `RepoNotes.App/Styles/AppTheme.axaml`.
- Keep visual-only mock elements clearly separated from business logic.

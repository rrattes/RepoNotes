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

## Current Density Targets

- Sidebar: around 250px to 260px, currently `252px`.
- Preview panel: around 320px to 340px, currently `326px`.
- Top bar: around 54px to 58px, currently `54px`.
- Status bar: compact, around 38px to 42px, currently `38px`.
- Editor column gets priority for all extra horizontal space.
- Editor chrome should stay compact: document tab around `32px`, title row around `50px`, and formatting toolbar around `34px` to `40px`.
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
- Centralize reusable colors and control styling in `RepoNotes.App/Styles/AppTheme.axaml`.
- Keep visual-only mock elements clearly separated from business logic.

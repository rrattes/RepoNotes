# Visual Markdown Editor Plan

## Purpose

This document plans a future Visual Markdown Mode / Visual Editor Mode for RepoNotes. It is a technical planning document only; no implementation is included in this round.

RepoNotes remains Markdown-first and local-first. Markdown files on disk stay the source of truth. The existing Markdown Mode must continue to exist and must not be replaced by a visual editor.

## Current Product Direction Update

Before any WebView2 or WYSIWYG implementation, RepoNotes is prioritizing a lower-risk Markdown Power Editor path. The first implemented step in that path is Split View: the Markdown source editor and native rendered preview are shown side by side while Markdown remains the source of truth.

Split View is not WYSIWYG. It is an incremental alternative that improves writing feedback without adding a browser runtime, replacing the Avalonia `TextBox`, or changing storage. The WebView2 spike remains a future option for real Visual Markdown Mode, but it is not the immediate priority.

## Product Decision

- Markdown remains the saved file format.
- The user can switch between Markdown Mode and Visual Mode.
- Visual Mode writes changes back to Markdown.
- Existing repositories and `.md` files must keep working.
- Manual Markdown editing must remain available.
- Visual Mode must work with open note tabs.
- Dirty/saved state remains per tab.
- Preview, metadata, internal links, and frontmatter must stay consistent with the active tab.
- YAML frontmatter is not edited as rich text. It remains managed by parsing/storage and the Info panel.

## Existing Architecture Context

- `MainWindow.axaml` currently switches the central area between `Editor` and `Preview`.
- `MainWindowViewModel` exposes editor/preview mode, active note tab state, Markdown text, save state, and toolbar formatting commands.
- `MarkdownPreviewService` converts Markdown into native preview block view models using Markdig.
- `MarkdownInlineTextBlock` renders inline preview runs with Avalonia `TextBlock.Inlines`.
- The editor is currently a plain Avalonia `TextBox`; it is intentionally Markdown source editing, not WYSIWYG.

## Future Minimum Scope

The first future Visual Mode should support:

- H1, H2, H3 headings.
- Bold.
- Italic.
- Bold italic.
- Lists.
- Checklists.
- Blockquotes.
- Inline code.
- Code blocks.
- Visual links.

## Initial Out Of Scope

- Advanced WYSIWYG tables.
- Images.
- Drag and drop.
- Rich paste from Word/HTML.
- Multiple cursors.
- Real-time collaboration.
- Plugin system.
- AI features.
- Visual editing of YAML frontmatter.
- Advanced export.
- Sync.

## Option 1: Native Avalonia Editor

Build a custom visual editor using Avalonia controls, text layout primitives, or a custom control. Markdown parsing would map source text to editable visual spans and block elements.

### Evaluation

- Complexity: Very high. A real editor needs cursor movement, text selection, IME support, undo/redo, clipboard, keyboard navigation, scroll anchoring, accessibility, and block editing behavior.
- Risk: High. It can become a custom text editor project inside RepoNotes.
- Experience quality: Potentially good if implemented deeply, but a shallow version risks feeling worse than the current Markdown TextBox.
- Build impact: Low to medium. It stays within Avalonia/.NET and avoids browser dependencies.
- Self-contained packaging impact: Low. No external runtime dependency beyond the app.
- MVVM impact: Medium. The ViewModel can keep Markdown as state, but the View needs richer editing state and event handling.
- Test impact: Medium to high. Pure transformations are testable, but cursor/selection behavior needs UI-level tests or careful control tests.
- Markdown compatibility: Good if Markdown is always regenerated from the same document model. Risk appears when preserving exact source formatting, whitespace, and unsupported Markdown.
- Formatting support difficulty:
  - Headings: Medium.
  - Bold/italic/bold italic: Medium.
  - Lists/checklists: High because block editing, continuation, indentation, and Enter behavior matter.
  - Code/code blocks: Medium to high.
  - Links: Medium.
  - Tables: Very high for any true visual editing.

### Notes

This option keeps the stack pure and packaging simple, but it is the most likely to drain engineering time. It is better suited to a deliberately limited WYSIWYG-lite control than a full rich editor.

## Option 2: WebView2 With Embedded Web Editor

Embed a proven web-based Markdown editor or ProseMirror/CodeMirror/TipTap-style editor inside WebView2, using a bridge between Avalonia and the web editor. Markdown remains the source format saved by RepoNotes.

### Evaluation

- Complexity: Medium to high. A mature editor provides editing behavior, but the app must own WebView lifecycle, JS/.NET bridge, commands, dirty state, theme sync, focus, and packaging.
- Risk: Medium to high. Main risks are WebView2 runtime behavior, bridge bugs, asset bundling, native packaging, and keeping Markdown conversion predictable.
- Experience quality: Highest potential. Web editor ecosystems already solve cursoring, selection, lists, links, undo/redo, paste, and rich editing details.
- Build impact: Medium. It adds browser/editor assets and likely new package/runtime assumptions.
- Self-contained packaging impact: Medium to high. Need to decide whether relying on installed WebView2 runtime is acceptable or whether fixed runtime packaging is required.
- MVVM impact: Medium. ViewModel remains Markdown/dirty-state owner, but visual editing state lives inside the WebView and syncs through commands/events.
- Test impact: Medium to high. Markdown conversion can be unit tested, but editor behavior needs integration tests, browser automation, or targeted JS tests.
- Markdown compatibility: Depends on editor choice. Some visual editors normalize Markdown, reorder whitespace, or drop unsupported syntax. Must be evaluated with frontmatter, internal links, checklists, code blocks, and tables.
- Formatting support difficulty:
  - Headings: Low.
  - Bold/italic/bold italic: Low.
  - Lists/checklists: Low to medium.
  - Code/code blocks: Low to medium.
  - Links: Low.
  - Tables: Medium if the chosen editor supports Markdown tables; otherwise high.

### Notes

This is the strongest path for real WYSIWYG, but it must not be added directly into the app without a spike. The spike should verify runtime, packaging, Markdown round-trip, frontmatter handling, theme integration, commands, and tab dirty-state synchronization.

## Option 3: Hybrid Incremental Mode

Keep the current Markdown TextBox and rich native preview, then add small visual affordances incrementally. Examples: a stronger split/toggle workflow, source editor helpers, rendered block preview beside the caret, or limited visual block editing for selected blocks.

### Evaluation

- Complexity: Low to medium for first iterations.
- Risk: Low. It builds on the current `MarkdownPreviewService`, toolbar, tabs, and Markdown source editor.
- Experience quality: Medium. It improves usability but is not true WYSIWYG.
- Build impact: Low. No new runtime or browser dependency.
- Self-contained packaging impact: Low.
- MVVM impact: Low to medium. Existing ViewModel state can remain the owner of Markdown, dirty state, active tab, preview blocks, and commands.
- Test impact: Low to medium. Markdown transformations, preview blocks, and mode state are already testable.
- Markdown compatibility: High because Markdown remains directly editable and preview is derived from it.
- Formatting support difficulty:
  - Headings: Low.
  - Bold/italic/bold italic: Low for preview and toolbar, medium for visual editing.
  - Lists/checklists: Low for toolbar/preview, medium for helper behaviors.
  - Code/code blocks: Low for preview and toolbar, medium for visual editing.
  - Links: Low for visual rendering, medium for inline editing.
  - Tables: Medium for preview, high for visual editing.

### Notes

This option is the safest incremental path. It can reduce friction while preserving the current architecture, but it should not be marketed internally as full WYSIWYG.

## Recommended Direction

Recommended primary path:

1. Keep the current Markdown Mode and central Preview mode as the stable baseline.
2. Plan a short isolated WebView2/editor spike before committing to WebView2 in the product.
3. If the spike proves WebView2 packaging, Markdown round-trip, theme integration, and MVVM sync are acceptable, use WebView2 as the future path for real Visual Markdown Mode.
4. If the spike is too heavy or unstable, continue with a native/hybrid WYSIWYG-lite path that improves Markdown editing without pretending to be full WYSIWYG.

This recommendation matches the product preference: WebView2 is the best candidate for real WYSIWYG quality, but only after technical proof. RepoNotes should not absorb a browser/editor dependency until the spike demonstrates value and low enough risk.

## Required Spike Checklist

The WebView2/editor spike should be separate from normal feature work and should not modify storage. It should prove:

- Avalonia WebView2 hosting works reliably on the target Windows version.
- The editor can load Markdown from the active tab.
- The editor can emit Markdown back to the ViewModel.
- Dirty state is updated per tab without corrupting other open tabs.
- `Ctrl+S` saves the active tab Markdown.
- Switching Markdown Mode / Visual Mode keeps content stable.
- YAML frontmatter is excluded from rich editing and preserved on save.
- Existing Markdown files without frontmatter still open and save.
- Theme matches the dark UI guide.
- Self-contained packaging strategy is understood.
- Build and CI implications are documented.

## Architecture Guardrails

- Do not make Visual Mode the only editing mode.
- Do not save HTML as the note source.
- Do not hide or rewrite frontmatter as visual document content.
- Keep Markdown serialization deterministic enough for local files and Git diffs.
- Keep visual editor state scoped to the active tab.
- Avoid storage changes until the editor spike passes.
- Keep tests centered on Markdown round-trip, dirty state, and mode switching.

## First Implementation Slice After Spike

If approved after the spike, the first product slice should be:

- Add `Markdown Mode` / `Visual Mode` as a mode pair, keeping the current `Preview` available or folded into the mode model intentionally.
- Load active tab Markdown into Visual Mode.
- Edit H1/H2/H3, bold, italic, bold italic, lists, checklist, quote, inline code, code block, and visual links.
- Save back to Markdown through the existing save command.
- Preserve frontmatter and update metadata through the existing Info panel.
- Add tests for mode switching, tab dirty state, Markdown round-trip, and frontmatter preservation.

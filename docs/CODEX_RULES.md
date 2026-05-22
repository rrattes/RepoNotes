# Codex Development Rules

## General

- Keep Avalonia UI as the desktop framework.
- Preserve MVVM.
- Prefer small, verifiable changes.
- Do not add login, cloud, AI, sync, or database features unless explicitly requested.
- Do not change business logic during visual-only rounds.
- Avoid spreading visual logic into code-behind.

## Build And Validation

- Run `dotnet build` and `dotnet test` before finishing each development round.
- If using the local SDK installed in this workspace (Windows), run:

```powershell
.\.dotnet\dotnet.exe build RepoNotes.sln
.\.dotnet\dotnet.exe test RepoNotes.sln --no-build
```

- In remote Linux sessions (Claude Code on the web), the .NET SDK is not available locally.
  In that case, push to the remote and let GitHub Actions CI validate build and tests.
  CI runs automatically on every push to `main` and on every pull request via `.github/workflows/ci.yml`.
- Record the build/test result (or CI status) in `docs/TASK_LOG.md`.
- Never report a round as complete without confirmed build success — either local or CI green.

## Task Logging

Every development round must update `docs/TASK_LOG.md` with:

- Date/time.
- Objective of the round.
- Files changed.
- Summary of changes.
- `dotnet build` result.
- Open items.
- Technical risks.
- Suggested next step.

## Git

- Make small commits with clear messages.
- Push completed work to `https://github.com/rrattes/RepoNotes`.
- Do not commit local SDK files, build outputs, or IDE state.
- Keep `.gitignore` aligned with generated files.

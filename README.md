# RepoNotes

RepoNotes is a local-first desktop notes MVP built with .NET and Avalonia UI.

## Projects

- `RepoNotes.App`: Avalonia UI desktop app using MVVM.
- `RepoNotes.Core`: Domain models and repository contracts.
- `RepoNotes.Storage`: Mock local note repository.
- `RepoNotes.Tests`: xUnit tests.

## Run

```powershell
.\.dotnet\dotnet.exe run --project .\RepoNotes.App
```

If you have the .NET SDK installed globally:

```powershell
dotnet run --project .\RepoNotes.App
```

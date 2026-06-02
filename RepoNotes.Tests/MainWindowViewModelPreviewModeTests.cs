using RepoNotes.App.ViewModels;
using RepoNotes.Storage;

namespace RepoNotes.Tests;

public sealed class MainWindowViewModelPreviewModeTests
{
    private static MainWindowViewModel CreateViewModel() =>
        new(new MockNoteRepository());

    [Fact]
    public void StartsInEditorMode()
    {
        var viewModel = CreateViewModel();

        Assert.True(viewModel.IsEditorMode);
        Assert.False(viewModel.IsPreviewMode);
    }

    [Fact]
    public void ShowPreviewCommandSwitchesToPreviewMode()
    {
        var viewModel = CreateViewModel();

        viewModel.ShowPreviewCommand.Execute(null);

        Assert.False(viewModel.IsEditorMode);
        Assert.True(viewModel.IsPreviewMode);
    }

    [Fact]
    public void ShowEditorCommandSwitchesBackToEditorMode()
    {
        var viewModel = CreateViewModel();

        viewModel.ShowPreviewCommand.Execute(null);
        viewModel.ShowEditorCommand.Execute(null);

        Assert.True(viewModel.IsEditorMode);
        Assert.False(viewModel.IsPreviewMode);
    }

    [Fact]
    public void SwitchingModeDoesNotChangeMarkdown()
    {
        var viewModel = CreateViewModel();
        var markdown = viewModel.Markdown;

        viewModel.ShowPreviewCommand.Execute(null);
        viewModel.ShowEditorCommand.Execute(null);

        Assert.Equal(markdown, viewModel.Markdown);
    }

    [Fact]
    public void EditingMarkdownKeepsPreviewBlocksUpdated()
    {
        var viewModel = CreateViewModel();

        viewModel.Markdown = "# Novo titulo";

        Assert.Contains(viewModel.PreviewBlocks, block => block is MarkdownHeadingBlock
        {
            Text: "Novo titulo",
            Level: 1
        });
    }
}

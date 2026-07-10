namespace AppFactory.Mobile.Tests;

public sealed class AiSuggestionWorkflowTests
{
    [Fact]
    public void LocalAiSuggestions_RequireExplicitQuizConfirmation()
    {
        var root = GetRepoRoot();
        var panel = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Mobile", "Components", "LocalAiPanel.razor"));
        var quiz = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Mobile", "Pages", "Quiz.razor"));
        var program = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Mobile", "MauiProgram.cs"));

        Assert.Contains("StoreImageSuggestions", panel, StringComparison.Ordinal);
        Assert.Contains("StoreAudioSuggestions", panel, StringComparison.Ordinal);
        Assert.Contains("Sugestia czeka w quizie", panel, StringComparison.Ordinal);

        Assert.Contains("AiSuggestionStateService", quiz, StringComparison.Ordinal);
        Assert.Contains("UseSuggestion", quiz, StringComparison.Ordinal);
        Assert.Contains("Użyj tej sugestii", quiz, StringComparison.Ordinal);
        Assert.Contains("optionExists", quiz, StringComparison.Ordinal);
        Assert.Contains("SuggestionState.Remove", quiz, StringComparison.Ordinal);

        Assert.Contains("AddSingleton<AiSuggestionStateService>", program, StringComparison.Ordinal);
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "projects")) && Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}

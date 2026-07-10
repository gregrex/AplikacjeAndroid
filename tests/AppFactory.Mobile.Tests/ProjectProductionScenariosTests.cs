using System.Text.RegularExpressions;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed partial class ProjectProductionScenariosTests
{
    [Fact]
    public void EveryCatalogProject_HasExactlyFiveCompleteProductionScenarios()
    {
        var root = GetRepoRoot();
        var projects = new ProjectCatalogService().GetProjects();
        var errors = new List<string>();

        foreach (var project in projects)
        {
            var path = Path.Combine(root, "projects", project.Id, "tests", "production-scenarios.md");
            if (!File.Exists(path))
            {
                errors.Add($"{project.Id}: missing production-scenarios.md");
                continue;
            }

            var content = File.ReadAllText(path);
            var matches = ScenarioHeaderRegex().Matches(content);
            if (matches.Count != 5)
            {
                errors.Add($"{project.Id}: expected 5 scenarios, found {matches.Count}");
                continue;
            }

            var ids = matches.Cast<Match>().Select(match => match.Groups[1].Value).ToArray();
            var expectedIds = new[] { "01", "02", "03", "04", "05" };
            if (!ids.SequenceEqual(expectedIds))
            {
                errors.Add($"{project.Id}: expected scenario ids SCN-01..SCN-05, found {string.Join(", ", ids.Select(x => $"SCN-{x}"))}");
            }

            for (var index = 0; index < matches.Count; index++)
            {
                var start = matches[index].Index;
                var end = index + 1 < matches.Count ? matches[index + 1].Index : content.Length;
                var block = content[start..end];
                var scenarioId = $"SCN-{ids[index]}";

                RequireMarker(errors, project.Id, scenarioId, block, "**Cel:**");
                RequireMarker(errors, project.Id, scenarioId, block, "**Kroki:**");
                RequireMarker(errors, project.Id, scenarioId, block, "**Oczekiwany wynik:**");
                RequireMarker(errors, project.Id, scenarioId, block, "**Pokrycie:**");

                var stepCount = NumberedStepRegex().Matches(block).Count;
                if (stepCount < 2)
                {
                    errors.Add($"{project.Id}/{scenarioId}: expected at least 2 numbered steps, found {stepCount}");
                }
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void ImageProjects_HaveOnDeviceVisionScenario()
    {
        var root = GetRepoRoot();
        var projectIds = new ImageAnalysisPolicyService().GetEnabledProjectIds();
        var errors = new List<string>();

        foreach (var projectId in projectIds)
        {
            var content = ReadScenarioFile(root, projectId);
            if (!content.Contains("local-vision-v1", StringComparison.OrdinalIgnoreCase)
                || !content.Contains("ONNX", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{projectId}: missing local-vision-v1 ONNX scenario");
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    [Fact]
    public void AudioProjects_HaveOnDeviceAudioScenario()
    {
        var root = GetRepoRoot();
        var projectIds = new AudioAnalysisPolicyService().GetEnabledProjectIds();
        var errors = new List<string>();

        foreach (var projectId in projectIds)
        {
            var content = ReadScenarioFile(root, projectId);
            if (!content.Contains("local-audio-v1", StringComparison.OrdinalIgnoreCase)
                || !content.Contains("ONNX", StringComparison.OrdinalIgnoreCase))
            {
                errors.Add($"{projectId}: missing local-audio-v1 ONNX scenario");
            }
        }

        Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
    }

    private static string ReadScenarioFile(string root, string projectId)
    {
        var path = Path.Combine(root, "projects", projectId, "tests", "production-scenarios.md");
        return File.ReadAllText(path);
    }

    private static void RequireMarker(List<string> errors, string projectId, string scenarioId, string block, string marker)
    {
        if (!block.Contains(marker, StringComparison.Ordinal))
        {
            errors.Add($"{projectId}/{scenarioId}: missing {marker}");
        }
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

    [GeneratedRegex(@"^## SCN-(\d{2})\s+—\s+.+$", RegexOptions.Multiline)]
    private static partial Regex ScenarioHeaderRegex();

    [GeneratedRegex(@"^\d+\.\s+.+$", RegexOptions.Multiline)]
    private static partial Regex NumberedStepRegex();
}

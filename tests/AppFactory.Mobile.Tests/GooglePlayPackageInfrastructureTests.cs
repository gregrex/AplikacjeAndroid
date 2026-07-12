using System.Text.Json;

namespace AppFactory.Mobile.Tests;

public sealed class GooglePlayPackageInfrastructureTests
{
    [Fact]
    public void ReleaseAutomation_HasPreparationVerificationSigningAndCi()
    {
        var root = GetRepoRoot();
        var required = new[]
        {
            "tools/release/prepare-google-play-package.ps1",
            "tools/release/verify-google-play-package.ps1",
            "tools/release/create-upload-keystore.ps1",
            "tools/release/export-keystore-for-github.ps1",
            "tools/release/build-play-aab.ps1",
            "tools/release/generate-play-graphics.ps1",
            "tools/release/export-play-metadata.ps1",
            "tools/release/capture-play-screenshot.ps1",
            ".github/workflows/release-readiness.yml",
            ".github/workflows/release-aab.yml",
            ".github/workflows/pages.yml"
        };

        var missing = required
            .Where(path => !File.Exists(Path.Combine(root, path.Replace('/', Path.DirectorySeparatorChar))))
            .ToArray();

        Assert.True(missing.Length == 0, "Missing Google Play automation:" + Environment.NewLine + string.Join(Environment.NewLine, missing));
    }

    [Fact]
    public void PlayConsoleConfig_MatchesVersionOneReleaseContract()
    {
        var root = GetRepoRoot();
        var path = Path.Combine(root, "marketing", "google-play", "play-console-config.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var config = document.RootElement;
        var application = config.GetProperty("application");
        var release = config.GetProperty("release");
        var appContent = config.GetProperty("appContent");

        Assert.Equal("AppFactory Pomocniki", application.GetProperty("name").GetString());
        Assert.Equal("pl.gbcom.appfactory", application.GetProperty("packageName").GetString());
        Assert.Equal("FREE", application.GetProperty("pricing").GetString());
        Assert.Equal("TOOLS", application.GetProperty("category").GetString());
        Assert.Equal("1.0.0", release.GetProperty("displayVersion").GetString());
        Assert.Equal(1, release.GetProperty("versionCode").GetInt32());
        Assert.True(release.GetProperty("targetApi").GetInt32() >= 35);
        Assert.False(release.GetProperty("localAiEnabled").GetBoolean());
        Assert.False(release.GetProperty("ads").GetBoolean());
        Assert.False(release.GetProperty("inAppPurchases").GetBoolean());
        Assert.False(release.GetProperty("requiresAccount").GetBoolean());
        Assert.False(release.GetProperty("externalAnalytics").GetBoolean());
        Assert.Equal("NO_ADS", appContent.GetProperty("adsDeclaration").GetString());
        Assert.Equal("ADULTS_ONLY", appContent.GetProperty("targetAudience").GetString());
        Assert.False(appContent.GetProperty("dataCollectedOffDevice").GetBoolean());
        Assert.False(appContent.GetProperty("dataShared").GetBoolean());
    }

    [Fact]
    public void ReleaseScripts_KeepSecretsOutsideRepositoryAndAiDisabled()
    {
        var root = GetRepoRoot();
        var build = File.ReadAllText(Path.Combine(root, "tools", "release", "build-play-aab.ps1"));
        var workflow = File.ReadAllText(Path.Combine(root, ".github", "workflows", "release-aab.yml"));
        var gitignore = File.ReadAllText(Path.Combine(root, ".gitignore"));

        Assert.Contains("Signing secret must be stored outside the repository", build);
        Assert.Contains("EnableLocalAiRelease=$localAiValue", build);
        Assert.Contains("ANDROID_KEYSTORE_BASE64", workflow);
        Assert.Contains("ANDROID_KEYSTORE_PASSWORD", workflow);
        Assert.Contains("ANDROID_KEY_PASSWORD", workflow);
        Assert.Contains("*.keystore", gitignore);
        Assert.Contains("*password*.txt", gitignore);
    }

    private static string GetRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "projects"))
                && Directory.Exists(Path.Combine(directory.FullName, "src")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}

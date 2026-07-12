using System.Text.Json;

namespace AppFactory.Mobile.Tests;

public sealed class GooglePlayReleaseReadinessTests
{
    [Fact]
    public void MobileProject_HasFinalGooglePlayIdentityAndPackaging()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var content = File.ReadAllText(Path.Combine(mobile, "AppFactory.Mobile.csproj"));
        var manifest = File.ReadAllText(Path.Combine(mobile, "Platforms", "Android", "AndroidManifest.xml"));

        Assert.Contains("<ApplicationTitle>AppFactory Pomocniki</ApplicationTitle>", content);
        Assert.Contains("<ApplicationId>pl.gbcom.appfactory</ApplicationId>", content);
        Assert.Contains("<ApplicationDisplayVersion>1.0.0</ApplicationDisplayVersion>", content);
        Assert.Contains("<ApplicationVersion>1</ApplicationVersion>", content);
        Assert.Contains("<AndroidTargetSdkVersion>35</AndroidTargetSdkVersion>", content);
        Assert.Contains("<AndroidPackageFormats>aab</AndroidPackageFormats>", content);
        Assert.Contains("<MauiIcon", content);
        Assert.Contains("<MauiSplashScreen", content);
        Assert.Contains("<EnableLocalAiRelease>false</EnableLocalAiRelease>", content);
        Assert.Contains("android:allowBackup=\"false\"", manifest);
        Assert.Contains("android:usesCleartextTraffic=\"false\"", manifest);
    }

    [Fact]
    public void ReleaseBuild_HasNoMockAdsAndFullResultsAreAvailable()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var resultPage = File.ReadAllText(Path.Combine(mobile, "Pages", "Result.razor"));
        var program = File.ReadAllText(Path.Combine(mobile, "MauiProgram.cs"));
        var flags = File.ReadAllText(Path.Combine(mobile, "Services", "AppFeatureFlags.cs"));

        Assert.False(File.Exists(Path.Combine(mobile, "Services", "MockAdService.cs")));
        Assert.DoesNotContain("MockAdService", resultPage);
        Assert.DoesNotContain("ShowRewardedAsync", resultPage);
        Assert.DoesNotContain("MockAdService", program);
        Assert.Contains("AdsEnabled => false", flags);
        Assert.Contains("FullResultsEnabled => true", flags);
        Assert.Contains("Wszystkie kroki są dostępne", resultPage);
    }

    [Fact]
    public void StoreListings_CoverEuOfficialLanguagesAndUkrainianWithinLimits()
    {
        var root = GetRepoRoot();
        var path = Path.Combine(root, "marketing", "google-play", "listings.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var rootElement = document.RootElement;

        Assert.Equal("pl.gbcom.appfactory", rootElement.GetProperty("packageName").GetString());
        Assert.Equal("pl-PL", rootElement.GetProperty("defaultLocale").GetString());
        Assert.Equal("TOOLS", rootElement.GetProperty("category").GetString());
        Assert.True((rootElement.GetProperty("privacyPolicyUrl").GetString() ?? string.Empty).StartsWith("https://", StringComparison.Ordinal));
        Assert.True((rootElement.GetProperty("supportUrl").GetString() ?? string.Empty).StartsWith("https://", StringComparison.Ordinal));

        var expectedLocales = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "bg-BG", "hr-HR", "cs-CZ", "da-DK", "nl-NL", "en-US", "et-EE", "fi-FI",
            "fr-FR", "de-DE", "el-GR", "hu-HU", "ga-IE", "it-IT", "lv-LV", "lt-LT",
            "mt-MT", "pl-PL", "pt-PT", "ro-RO", "sk-SK", "sl-SI", "es-ES", "sv-SE", "uk-UA"
        };

        var locales = rootElement.GetProperty("locales");
        var actualLocales = locales.EnumerateObject().Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Equal(expectedLocales.Count, actualLocales.Count);
        Assert.True(expectedLocales.SetEquals(actualLocales));

        foreach (var locale in locales.EnumerateObject())
        {
            var title = locale.Value.GetProperty("title").GetString() ?? string.Empty;
            var shortDescription = locale.Value.GetProperty("shortDescription").GetString() ?? string.Empty;
            var fullDescription = locale.Value.GetProperty("fullDescription").GetString() ?? string.Empty;

            Assert.InRange(title.Length, 1, 30);
            Assert.InRange(shortDescription.Length, 1, 80);
            Assert.InRange(fullDescription.Length, 1, 4000);
            Assert.False(shortDescription.Contains("#1", StringComparison.OrdinalIgnoreCase));
            Assert.False(shortDescription.Contains("best", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void FirstRelease_ExportsOnlyLanguagesWithCompleteInAppContent()
    {
        var root = GetRepoRoot();
        var path = Path.Combine(root, "marketing", "google-play", "release-locales.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));

        var releaseLocales = document.RootElement.GetProperty("releaseLocales")
            .EnumerateArray()
            .Select(x => x.GetString() ?? string.Empty)
            .ToArray();
        var plannedLocales = document.RootElement.GetProperty("plannedLocales")
            .EnumerateArray()
            .Select(x => x.GetString() ?? string.Empty)
            .ToArray();

        Assert.Equal(new[] { "pl-PL", "en-US", "uk-UA" }, releaseLocales);
        Assert.Equal(22, plannedLocales.Length);
        Assert.Equal(25, releaseLocales.Concat(plannedLocales).Distinct(StringComparer.OrdinalIgnoreCase).Count());

        var exporter = File.ReadAllText(Path.Combine(root, "tools", "release", "export-play-metadata.ps1"));
        Assert.Contains("releaseLocales", exporter);
        Assert.Contains("IncludePlannedLocales", exporter);
    }

    [Fact]
    public void LegalMarketingAndSupportInfrastructure_IsComplete()
    {
        var root = GetRepoRoot();
        var required = new[]
        {
            "site/index.html",
            "site/privacy/index.html",
            "site/support/index.html",
            "site/terms/index.html",
            "site/assets/logo.svg",
            "site/assets/styles.css",
            ".github/workflows/pages.yml",
            ".github/workflows/release-aab.yml",
            "marketing/brand/BRAND_GUIDE.md",
            "marketing/google-play/listings.json",
            "marketing/google-play/release-locales.json",
            "marketing/google-play/source/store-icon.svg",
            "marketing/google-play/source/feature-graphic.svg",
            "marketing/google-play/SCREENSHOT_PLAN.md",
            "docs/release/GOOGLE_PLAY_RELEASE_PLAN.md",
            "docs/release/PLAY_CONSOLE_CHECKLIST.md",
            "docs/release/DATA_SAFETY_DECLARATION.md",
            "docs/release/CONTENT_RATING_GUIDE.md",
            "tools/release/create-upload-keystore.ps1",
            "tools/release/build-play-aab.ps1",
            "tools/release/generate-play-graphics.ps1",
            "tools/release/export-play-metadata.ps1",
            "tools/release/capture-play-screenshot.ps1"
        };

        var missing = required
            .Where(relative => !File.Exists(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar))))
            .ToArray();

        Assert.True(missing.Length == 0, "Missing release files:" + Environment.NewLine + string.Join(Environment.NewLine, missing));
    }

    [Fact]
    public void PublicPolicy_DescribesActualVersionOneBehavior()
    {
        var root = GetRepoRoot();
        var privacy = File.ReadAllText(Path.Combine(root, "site", "privacy", "index.html"));
        var dataSafety = File.ReadAllText(Path.Combine(root, "docs", "release", "DATA_SAFETY_DECLARATION.md"));

        Assert.True(privacy.Contains("nie zawiera reklam", StringComparison.OrdinalIgnoreCase));
        Assert.True(privacy.Contains("nie udostępnia analizy zdjęć ani dźwięku", StringComparison.OrdinalIgnoreCase));
        Assert.True(privacy.Contains("nie są wysyłane automatycznie", StringComparison.OrdinalIgnoreCase));
        Assert.Contains("AdsEnabled=false", dataSafety);
        Assert.Contains("EnableLocalAiRelease=false", dataSafety);
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

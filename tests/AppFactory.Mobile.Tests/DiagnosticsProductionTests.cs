using AppFactory.Persistence.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Tests;

public sealed class DiagnosticsProductionTests
{
    [Fact]
    public void LocalDiagnosticsInfrastructure_IsRegisteredAndVisible()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var program = File.ReadAllText(Path.Combine(mobile, "MauiProgram.cs"));
        var app = File.ReadAllText(Path.Combine(mobile, "App.xaml.cs"));
        var settings = File.ReadAllText(Path.Combine(mobile, "Pages", "Settings.razor"));
        var diagnostics = File.ReadAllText(Path.Combine(mobile, "Pages", "Diagnostics.razor"));
        var runner = File.ReadAllText(Path.Combine(root, "tools", "quality", "run-local-test-plan.ps1"));

        Assert.Contains("LocalLogStore", program);
        Assert.Contains("LocalFileLoggerProvider", program);
        Assert.Contains("DiagnosticsExportService", program);
        Assert.Contains("UnhandledException", app);
        Assert.Contains("UnobservedTaskException", app);
        Assert.Contains("href=\"/diagnostics\"", settings);
        Assert.Contains("Eksportuj i udostępnij ZIP", diagnostics);
        Assert.Contains("LOCAL_TEST_MARKER", diagnostics);
        Assert.Contains("adb-logcat-snapshot", runner);
    }

    [Fact]
    public void LoggingPolicy_HasRotationRetentionAndPrivacyDefaults()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"appfactory-diagnostics-contract-{Guid.NewGuid():N}");
        try
        {
            var store = new LocalLogStore(directory, new LocalLogOptions
            {
                MinimumLevel = LogLevel.Information,
                RetentionDays = 7,
                MaxFiles = 12,
                MaxFileSizeBytes = 2 * 1024 * 1024
            });

            Assert.Equal(7, store.Options.RetentionDays);
            Assert.Equal(12, store.Options.MaxFiles);
            Assert.Equal(2 * 1024 * 1024, store.Options.MaxFileSizeBytes);
            Assert.Equal(LogLevel.Information, store.Options.MinimumLevel);

            var sanitized = LogSanitizer.Sanitize(
                "person@example.com token=abc Bearer secret-value",
                1000);

            Assert.Contains("[REDACTED_EMAIL]", sanitized);
            Assert.Contains("[REDACTED]", sanitized);
            Assert.DoesNotContain("person@example.com", sanitized);
            Assert.DoesNotContain("secret-value", sanitized);
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }

    [Fact]
    public void DiagnosticsBundle_IsManualAndContainsDeviceDatabaseAndLogs()
    {
        var root = GetRepoRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src",
            "AppFactory.Mobile",
            "Services",
            "DiagnosticsExportService.cs"));

        Assert.Contains("Share.Default.RequestAsync", service);
        Assert.Contains("diagnostics-manifest.json", service);
        Assert.Contains("DeviceInfo.Current", service);
        Assert.Contains("AppInfo.Current", service);
        Assert.Contains("Database = databaseHealth", service);
        Assert.Contains("logs/", service);
        Assert.DoesNotContain("HttpClient", service);
    }

    private static string GetRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "projects"))
                && Directory.Exists(Path.Combine(dir.FullName, "src")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}

using System.IO.Compression;
using System.Text.Json;
using AppFactory.Persistence;
using AppFactory.Persistence.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class DiagnosticsExportService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly LocalLogStore _logs;
    private readonly LocalDatabaseService _database;
    private readonly ILogger<DiagnosticsExportService> _logger;

    public DiagnosticsExportService(
        LocalLogStore logs,
        LocalDatabaseService database,
        ILogger<DiagnosticsExportService> logger)
    {
        _logs = logs;
        _database = database;
        _logger = logger;
    }

    public async Task<DiagnosticsStatus> GetStatusAsync()
    {
        AppDatabaseHealth? databaseHealth = null;
        string databaseError = string.Empty;

        try
        {
            databaseHealth = await _database.GetHealthAsync();
        }
        catch (Exception ex)
        {
            databaseError = ex.Message;
            _logger.LogError(ex, "Diagnostics database health check failed.");
        }

        return new DiagnosticsStatus
        {
            Logs = _logs.GetSnapshot(),
            Database = databaseHealth,
            DatabaseError = databaseError,
            Tail = _logs.ReadTail(100)
        };
    }

    public void WriteTestMarker()
    {
        _logger.LogInformation("LOCAL_TEST_MARKER session={SessionId} utc={TimestampUtc}", _logs.SessionId, DateTime.UtcNow);
    }

    public async Task<string> CreateBundleAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating local diagnostics bundle.");
        _logs.Cleanup();

        var bundlePath = Path.Combine(
            FileSystem.CacheDirectory,
            $"appfactory-diagnostics-{DateTime.UtcNow:yyyyMMdd-HHmmss}.zip");

        if (File.Exists(bundlePath))
        {
            File.Delete(bundlePath);
        }

        var databaseHealth = await TryGetDatabaseHealthAsync();
        var manifest = new DiagnosticsManifest
        {
            CreatedAtUtc = DateTime.UtcNow,
            SessionId = _logs.SessionId,
            ApplicationName = AppInfo.Current.Name,
            ApplicationVersion = AppInfo.Current.VersionString,
            ApplicationBuild = AppInfo.Current.BuildString,
            PackageName = AppInfo.Current.PackageName,
            DeviceManufacturer = DeviceInfo.Current.Manufacturer,
            DeviceModel = DeviceInfo.Current.Model,
            DeviceName = DeviceInfo.Current.Name,
            DevicePlatform = DeviceInfo.Current.Platform.ToString(),
            DeviceVersion = DeviceInfo.Current.VersionString,
            DeviceIdiom = DeviceInfo.Current.Idiom.ToString(),
            Database = databaseHealth,
            Logs = _logs.GetSnapshot(),
            PrivacyNotice = "Paczka jest tworzona lokalnie i udostępniana wyłącznie po ręcznej akcji użytkownika. Sekrety oraz adresy e-mail są maskowane."
        };

        await using var output = File.Create(bundlePath);
        using var archive = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: false);

        var manifestEntry = archive.CreateEntry("diagnostics-manifest.json", CompressionLevel.Optimal);
        await using (var manifestStream = manifestEntry.Open())
        {
            await JsonSerializer.SerializeAsync(manifestStream, manifest, JsonOptions, cancellationToken);
        }

        foreach (var logFile in _logs.GetFiles())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entry = archive.CreateEntry($"logs/{logFile.Name}", CompressionLevel.Optimal);
            await using var entryStream = entry.Open();
            await using var source = File.OpenRead(logFile.Path);
            await source.CopyToAsync(entryStream, cancellationToken);
        }

        _logger.LogInformation("Diagnostics bundle created: {FileName}", Path.GetFileName(bundlePath));
        return bundlePath;
    }

    public async Task ShareBundleAsync(CancellationToken cancellationToken = default)
    {
        var path = await CreateBundleAsync(cancellationToken);
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "AppFactory — logi diagnostyczne",
            File = new ShareFile(path)
        });
    }

    public void ClearLogs()
    {
        _logs.Clear();
        _logger.LogInformation("Local logs cleared by user. New session log started.");
    }

    private async Task<AppDatabaseHealth?> TryGetDatabaseHealthAsync()
    {
        try
        {
            return await _database.GetHealthAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not add database health to diagnostics bundle.");
            return null;
        }
    }
}

public sealed class DiagnosticsStatus
{
    public LocalLogSnapshot Logs { get; init; } = new();
    public AppDatabaseHealth? Database { get; init; }
    public string DatabaseError { get; init; } = string.Empty;
    public IReadOnlyList<string> Tail { get; init; } = Array.Empty<string>();
}

public sealed class DiagnosticsManifest
{
    public DateTime CreatedAtUtc { get; init; }
    public string SessionId { get; init; } = string.Empty;
    public string ApplicationName { get; init; } = string.Empty;
    public string ApplicationVersion { get; init; } = string.Empty;
    public string ApplicationBuild { get; init; } = string.Empty;
    public string PackageName { get; init; } = string.Empty;
    public string DeviceManufacturer { get; init; } = string.Empty;
    public string DeviceModel { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
    public string DevicePlatform { get; init; } = string.Empty;
    public string DeviceVersion { get; init; } = string.Empty;
    public string DeviceIdiom { get; init; } = string.Empty;
    public AppDatabaseHealth? Database { get; init; }
    public LocalLogSnapshot Logs { get; init; } = new();
    public string PrivacyNotice { get; init; } = string.Empty;
}

using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Persistence.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class HistoryService
{
    private const string LegacyStorageKey = "appfactory:history:v2";
    private const string MigrationFlagKey = "appfactory:history:sqlite-migrated:v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly LocalDatabaseService _databaseService;
    private readonly ILogger<HistoryService> _logger;
    private readonly SemaphoreSlim _migrationLock = new(1, 1);

    public HistoryService(LocalDatabaseService databaseService, ILogger<HistoryService> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task AddAsync(HistoryEntry entry)
    {
        await EnsureReadyAsync();
        await _databaseService.Database.AddHistoryAsync(ToRecord(entry));
        _logger.LogInformation(
            "History entry saved. Project={ProjectId} Category={CategoryId} Result={ResultId}",
            entry.ProjectId,
            entry.CategoryId,
            entry.ResultId);
    }

    public async Task<IReadOnlyList<HistoryEntry>> GetAllAsync()
    {
        await EnsureReadyAsync();
        var records = await _databaseService.Database.GetHistoryAsync();
        _logger.LogDebug("History loaded. Count={Count}", records.Count);
        return records.Select(ToEntry).ToArray();
    }

    public async Task ClearAsync()
    {
        await EnsureReadyAsync();
        await _databaseService.Database.ClearHistoryAsync();
        _logger.LogInformation("History cleared by user.");
    }

    private async Task EnsureReadyAsync()
    {
        await _databaseService.InitializeAsync();
        if (Preferences.Default.Get(MigrationFlagKey, false))
        {
            return;
        }

        await _migrationLock.WaitAsync();
        try
        {
            if (Preferences.Default.Get(MigrationFlagKey, false))
            {
                return;
            }

            var migratedCount = 0;
            var json = Preferences.Default.Get(LegacyStorageKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var entries = JsonSerializer.Deserialize<List<HistoryEntry>>(json, JsonOptions)
                                  ?? new List<HistoryEntry>();
                    foreach (var entry in entries.OrderBy(x => x.CreatedAt))
                    {
                        await _databaseService.Database.AddHistoryAsync(ToRecord(entry));
                        migratedCount++;
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Legacy history JSON was corrupt and has been discarded.");
                }
            }

            Preferences.Default.Remove(LegacyStorageKey);
            Preferences.Default.Set(MigrationFlagKey, true);
            _logger.LogInformation("History migration to SQLite completed. MigratedCount={MigratedCount}", migratedCount);
        }
        finally
        {
            _migrationLock.Release();
        }
    }

    private static HistoryRecord ToRecord(HistoryEntry entry) => new()
    {
        Id = string.IsNullOrWhiteSpace(entry.Id) ? Guid.NewGuid().ToString("N") : entry.Id,
        ProjectId = entry.ProjectId,
        CategoryId = entry.CategoryId,
        ResultId = entry.ResultId,
        FreeResultId = entry.FreeResultId,
        PremiumResultId = entry.PremiumResultId,
        CreatedAtUtcTicks = entry.CreatedAt.ToUniversalTime().Ticks
    };

    private static HistoryEntry ToEntry(HistoryRecord record) => new()
    {
        Id = record.Id,
        ProjectId = record.ProjectId,
        CategoryId = record.CategoryId,
        ResultId = record.ResultId,
        FreeResultId = record.FreeResultId,
        PremiumResultId = record.PremiumResultId,
        CreatedAt = new DateTime(record.CreatedAtUtcTicks, DateTimeKind.Utc)
    };
}

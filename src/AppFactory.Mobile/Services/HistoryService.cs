using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Persistence.Entities;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class HistoryService
{
    private const string LegacyStorageKey = "appfactory:history:v2";
    private const string MigrationFlagKey = "appfactory:history:sqlite-migrated:v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly LocalDatabaseService _databaseService;
    private readonly SemaphoreSlim _migrationLock = new(1, 1);

    public HistoryService(LocalDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task AddAsync(HistoryEntry entry)
    {
        await EnsureReadyAsync();
        await _databaseService.Database.AddHistoryAsync(ToRecord(entry));
    }

    public async Task<IReadOnlyList<HistoryEntry>> GetAllAsync()
    {
        await EnsureReadyAsync();
        var records = await _databaseService.Database.GetHistoryAsync();
        return records.Select(ToEntry).ToArray();
    }

    public async Task ClearAsync()
    {
        await EnsureReadyAsync();
        await _databaseService.Database.ClearHistoryAsync();
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
                    }
                }
                catch (JsonException)
                {
                    // Corrupt legacy data is discarded instead of blocking the application startup.
                }
            }

            Preferences.Default.Remove(LegacyStorageKey);
            Preferences.Default.Set(MigrationFlagKey, true);
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

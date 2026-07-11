using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Persistence.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class FavoritesService
{
    private const string LegacyStorageKey = "appfactory:favorites:v2";
    private const string MigrationFlagKey = "appfactory:favorites:sqlite-migrated:v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly LocalDatabaseService _databaseService;
    private readonly ILogger<FavoritesService> _logger;
    private readonly SemaphoreSlim _migrationLock = new(1, 1);

    public FavoritesService(LocalDatabaseService databaseService, ILogger<FavoritesService> logger)
    {
        _databaseService = databaseService;
        _logger = logger;
    }

    public async Task AddAsync(FavoriteEntry entry)
    {
        await EnsureReadyAsync();
        var added = await _databaseService.Database.AddFavoriteAsync(ToRecord(entry));
        _logger.LogInformation(
            "Favorite save requested. Added={Added} Project={ProjectId} Result={ResultId}",
            added,
            entry.ProjectId,
            entry.ResultId);
    }

    public async Task RemoveAsync(string entryId)
    {
        await EnsureReadyAsync();
        var removed = await _databaseService.Database.RemoveFavoriteAsync(entryId);
        _logger.LogInformation("Favorite removed. RemovedCount={RemovedCount}", removed);
    }

    public async Task<IReadOnlyList<FavoriteEntry>> GetAllAsync()
    {
        await EnsureReadyAsync();
        var records = await _databaseService.Database.GetFavoritesAsync();
        _logger.LogDebug("Favorites loaded. Count={Count}", records.Count);
        return records.Select(ToEntry).ToArray();
    }

    public async Task ClearAsync()
    {
        await EnsureReadyAsync();
        await _databaseService.Database.ClearFavoritesAsync();
        _logger.LogInformation("Favorites cleared by user.");
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
                    var entries = JsonSerializer.Deserialize<List<FavoriteEntry>>(json, JsonOptions)
                                  ?? new List<FavoriteEntry>();
                    foreach (var entry in entries.OrderBy(x => x.CreatedAt))
                    {
                        if (await _databaseService.Database.AddFavoriteAsync(ToRecord(entry)))
                        {
                            migratedCount++;
                        }
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Legacy favorites JSON was corrupt and has been discarded.");
                }
            }

            Preferences.Default.Remove(LegacyStorageKey);
            Preferences.Default.Set(MigrationFlagKey, true);
            _logger.LogInformation("Favorites migration to SQLite completed. MigratedCount={MigratedCount}", migratedCount);
        }
        finally
        {
            _migrationLock.Release();
        }
    }

    private static FavoriteRecord ToRecord(FavoriteEntry entry) => new()
    {
        Id = string.IsNullOrWhiteSpace(entry.Id) ? Guid.NewGuid().ToString("N") : entry.Id,
        ProjectId = entry.ProjectId,
        CategoryId = entry.CategoryId,
        ResultId = entry.ResultId,
        FreeResultId = entry.FreeResultId,
        PremiumResultId = entry.PremiumResultId,
        CreatedAtUtcTicks = entry.CreatedAt.ToUniversalTime().Ticks
    };

    private static FavoriteEntry ToEntry(FavoriteRecord record) => new()
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

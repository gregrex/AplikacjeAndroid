using System.Text.Json;
using AppFactory.Mobile.Models;
using AppFactory.Persistence.Entities;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class FavoritesService
{
    private const string LegacyStorageKey = "appfactory:favorites:v2";
    private const string MigrationFlagKey = "appfactory:favorites:sqlite-migrated:v1";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly LocalDatabaseService _databaseService;
    private readonly SemaphoreSlim _migrationLock = new(1, 1);

    public FavoritesService(LocalDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task AddAsync(FavoriteEntry entry)
    {
        await EnsureReadyAsync();
        await _databaseService.Database.AddFavoriteAsync(ToRecord(entry));
    }

    public async Task RemoveAsync(string entryId)
    {
        await EnsureReadyAsync();
        await _databaseService.Database.RemoveFavoriteAsync(entryId);
    }

    public async Task<IReadOnlyList<FavoriteEntry>> GetAllAsync()
    {
        await EnsureReadyAsync();
        var records = await _databaseService.Database.GetFavoritesAsync();
        return records.Select(ToEntry).ToArray();
    }

    public async Task ClearAsync()
    {
        await EnsureReadyAsync();
        await _databaseService.Database.ClearFavoritesAsync();
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
                    var entries = JsonSerializer.Deserialize<List<FavoriteEntry>>(json, JsonOptions)
                                  ?? new List<FavoriteEntry>();
                    foreach (var entry in entries.OrderBy(x => x.CreatedAt))
                    {
                        await _databaseService.Database.AddFavoriteAsync(ToRecord(entry));
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

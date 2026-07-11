using AppFactory.Persistence;
using AppFactory.Persistence.Entities;

namespace AppFactory.Mobile.Tests;

public sealed class AppDatabaseTests
{
    [Fact]
    public async Task InitializeAsync_CreatesCurrentSchemaAndHealthReport()
    {
        var path = CreateDatabasePath();
        try
        {
            await using var database = new AppDatabase(path);

            await database.InitializeAsync();
            var health = await database.GetHealthAsync();

            Assert.True(health.IsReady);
            Assert.Equal(AppDatabase.CurrentSchemaVersion, health.SchemaVersion);
            Assert.Equal(0, health.HistoryCount);
            Assert.Equal(0, health.FavoritesCount);
            Assert.True(File.Exists(path));
        }
        finally
        {
            DeleteDatabaseFiles(path);
        }
    }

    [Fact]
    public async Task History_DeduplicatesSameResultAndReturnsNewestFirst()
    {
        var path = CreateDatabasePath();
        try
        {
            await using var database = new AppDatabase(path);
            var older = History("one", "result-a", DateTime.UtcNow.AddMinutes(-10));
            var newerDuplicate = History("two", "result-a", DateTime.UtcNow.AddMinutes(-1));
            var newest = History("three", "result-b", DateTime.UtcNow);

            await database.AddHistoryAsync(older);
            await database.AddHistoryAsync(newerDuplicate);
            await database.AddHistoryAsync(newest);
            var entries = await database.GetHistoryAsync();

            Assert.Equal(2, entries.Count);
            Assert.Equal("three", entries[0].Id);
            Assert.Equal("two", entries[1].Id);
            Assert.DoesNotContain(entries, x => x.Id == "one");
        }
        finally
        {
            DeleteDatabaseFiles(path);
        }
    }

    [Fact]
    public async Task History_IsTrimmedToProductionLimit()
    {
        var path = CreateDatabasePath();
        try
        {
            await using var database = new AppDatabase(path);
            var start = DateTime.UtcNow.AddHours(-2);

            for (var index = 0; index < AppDatabase.MaxHistoryEntries + 5; index++)
            {
                await database.AddHistoryAsync(History(
                    $"history-{index}",
                    $"result-{index}",
                    start.AddMinutes(index)));
            }

            var entries = await database.GetHistoryAsync();

            Assert.Equal(AppDatabase.MaxHistoryEntries, entries.Count);
            Assert.Equal($"history-{AppDatabase.MaxHistoryEntries + 4}", entries[0].Id);
            Assert.DoesNotContain(entries, x => x.Id == "history-0");
        }
        finally
        {
            DeleteDatabaseFiles(path);
        }
    }

    [Fact]
    public async Task Favorites_DeduplicateRemoveAndClear()
    {
        var path = CreateDatabasePath();
        try
        {
            await using var database = new AppDatabase(path);
            var first = Favorite("favorite-1", "result-a", DateTime.UtcNow.AddMinutes(-1));
            var duplicate = Favorite("favorite-2", "result-a", DateTime.UtcNow);
            var second = Favorite("favorite-3", "result-b", DateTime.UtcNow.AddMinutes(1));

            Assert.True(await database.AddFavoriteAsync(first));
            Assert.False(await database.AddFavoriteAsync(duplicate));
            Assert.True(await database.AddFavoriteAsync(second));

            var entries = await database.GetFavoritesAsync();
            Assert.Equal(2, entries.Count);
            Assert.Equal("favorite-3", entries[0].Id);

            Assert.Equal(1, await database.RemoveFavoriteAsync("favorite-1"));
            Assert.Single(await database.GetFavoritesAsync());

            await database.ClearFavoritesAsync();
            Assert.Empty(await database.GetFavoritesAsync());
        }
        finally
        {
            DeleteDatabaseFiles(path);
        }
    }

    private static HistoryRecord History(string id, string resultId, DateTime createdAt) => new()
    {
        Id = id,
        ProjectId = "plama-ratownik",
        CategoryId = "coffee",
        ResultId = resultId,
        FreeResultId = $"{resultId}-free",
        PremiumResultId = $"{resultId}-premium",
        CreatedAtUtcTicks = createdAt.ToUniversalTime().Ticks
    };

    private static FavoriteRecord Favorite(string id, string resultId, DateTime createdAt) => new()
    {
        Id = id,
        ProjectId = "router-wifi-diagnosta",
        CategoryId = "wifi",
        ResultId = resultId,
        FreeResultId = $"{resultId}-free",
        PremiumResultId = $"{resultId}-premium",
        CreatedAtUtcTicks = createdAt.ToUniversalTime().Ticks
    };

    private static string CreateDatabasePath() =>
        Path.Combine(Path.GetTempPath(), $"appfactory-tests-{Guid.NewGuid():N}.db3");

    private static void DeleteDatabaseFiles(string path)
    {
        foreach (var candidate in new[] { path, $"{path}-shm", $"{path}-wal" })
        {
            if (File.Exists(candidate))
            {
                File.Delete(candidate);
            }
        }
    }
}

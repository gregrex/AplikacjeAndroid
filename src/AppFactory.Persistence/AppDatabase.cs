using AppFactory.Persistence.Entities;
using SQLite;

namespace AppFactory.Persistence;

public sealed class AppDatabase : IAsyncDisposable
{
    public const int CurrentSchemaVersion = 1;
    public const int MaxHistoryEntries = 100;

    private readonly SQLiteAsyncConnection _connection;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _initialized;

    public AppDatabase(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("Database path is required.", nameof(databasePath));
        }

        DatabasePath = databasePath;
        var directory = Path.GetDirectoryName(databasePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        SQLitePCL.Batteries_V2.Init();
        _connection = new SQLiteAsyncConnection(
            databasePath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
    }

    public string DatabasePath { get; }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        await _initializationLock.WaitAsync();
        try
        {
            if (_initialized)
            {
                return;
            }

            await _connection.CreateTableAsync<SchemaVersionRecord>();
            await _connection.CreateTableAsync<HistoryRecord>();
            await _connection.CreateTableAsync<FavoriteRecord>();

            var schema = await _connection.Table<SchemaVersionRecord>().FirstOrDefaultAsync();
            if (schema is null)
            {
                await _connection.InsertAsync(new SchemaVersionRecord
                {
                    Id = 1,
                    Version = CurrentSchemaVersion,
                    UpdatedAtUtcTicks = DateTime.UtcNow.Ticks
                });
            }
            else if (schema.Version < CurrentSchemaVersion)
            {
                schema.Version = CurrentSchemaVersion;
                schema.UpdatedAtUtcTicks = DateTime.UtcNow.Ticks;
                await _connection.UpdateAsync(schema);
            }

            _initialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public async Task AddHistoryAsync(HistoryRecord record)
    {
        ValidateRecord(record.ProjectId, record.ResultId);
        await InitializeAsync();

        var duplicates = await _connection.Table<HistoryRecord>()
            .Where(x => x.ProjectId == record.ProjectId
                        && x.CategoryId == record.CategoryId
                        && x.ResultId == record.ResultId)
            .ToListAsync();

        foreach (var duplicate in duplicates)
        {
            await _connection.DeleteAsync(duplicate);
        }

        await _connection.InsertAsync(record);
        await TrimHistoryAsync();
    }

    public async Task<IReadOnlyList<HistoryRecord>> GetHistoryAsync()
    {
        await InitializeAsync();
        return await _connection.Table<HistoryRecord>()
            .OrderByDescending(x => x.CreatedAtUtcTicks)
            .Take(MaxHistoryEntries)
            .ToListAsync();
    }

    public async Task ClearHistoryAsync()
    {
        await InitializeAsync();
        await _connection.DeleteAllAsync<HistoryRecord>();
    }

    public async Task<bool> AddFavoriteAsync(FavoriteRecord record)
    {
        ValidateRecord(record.ProjectId, record.ResultId);
        await InitializeAsync();

        var existing = await _connection.Table<FavoriteRecord>()
            .Where(x => x.ProjectId == record.ProjectId && x.ResultId == record.ResultId)
            .FirstOrDefaultAsync();

        if (existing is not null)
        {
            return false;
        }

        await _connection.InsertAsync(record);
        return true;
    }

    public async Task<IReadOnlyList<FavoriteRecord>> GetFavoritesAsync()
    {
        await InitializeAsync();
        return await _connection.Table<FavoriteRecord>()
            .OrderByDescending(x => x.CreatedAtUtcTicks)
            .ToListAsync();
    }

    public async Task<int> RemoveFavoriteAsync(string entryId)
    {
        if (string.IsNullOrWhiteSpace(entryId))
        {
            return 0;
        }

        await InitializeAsync();
        return await _connection.DeleteAsync<FavoriteRecord>(entryId);
    }

    public async Task ClearFavoritesAsync()
    {
        await InitializeAsync();
        await _connection.DeleteAllAsync<FavoriteRecord>();
    }

    public async Task<AppDatabaseHealth> GetHealthAsync()
    {
        await InitializeAsync();
        var schema = await _connection.Table<SchemaVersionRecord>().FirstOrDefaultAsync();

        return new AppDatabaseHealth
        {
            DatabasePath = DatabasePath,
            SchemaVersion = schema?.Version ?? 0,
            HistoryCount = await _connection.Table<HistoryRecord>().CountAsync(),
            FavoritesCount = await _connection.Table<FavoriteRecord>().CountAsync(),
            IsReady = schema?.Version == CurrentSchemaVersion
        };
    }

    private async Task TrimHistoryAsync()
    {
        var obsolete = await _connection.Table<HistoryRecord>()
            .OrderByDescending(x => x.CreatedAtUtcTicks)
            .Skip(MaxHistoryEntries)
            .ToListAsync();

        foreach (var item in obsolete)
        {
            await _connection.DeleteAsync(item);
        }
    }

    private static void ValidateRecord(string projectId, string resultId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new ArgumentException("ProjectId is required.", nameof(projectId));
        }

        if (string.IsNullOrWhiteSpace(resultId))
        {
            throw new ArgumentException("ResultId is required.", nameof(resultId));
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
        _initializationLock.Dispose();
    }
}

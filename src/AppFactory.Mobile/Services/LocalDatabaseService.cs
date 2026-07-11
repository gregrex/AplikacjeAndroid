using AppFactory.Persistence;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class LocalDatabaseService : IAsyncDisposable
{
    public const string DatabaseFileName = "appfactory.db3";

    private readonly AppDatabase _database;

    public LocalDatabaseService()
        : this(Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName))
    {
    }

    public LocalDatabaseService(string databasePath)
    {
        _database = new AppDatabase(databasePath);
    }

    public AppDatabase Database => _database;

    public Task InitializeAsync() => _database.InitializeAsync();

    public Task<AppDatabaseHealth> GetHealthAsync() => _database.GetHealthAsync();

    public ValueTask DisposeAsync() => _database.DisposeAsync();
}

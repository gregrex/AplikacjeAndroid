namespace AppFactory.Persistence;

public sealed class AppDatabaseHealth
{
    public string DatabasePath { get; init; } = string.Empty;
    public int SchemaVersion { get; init; }
    public int HistoryCount { get; init; }
    public int FavoritesCount { get; init; }
    public bool IsReady { get; init; }
}

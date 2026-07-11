using AppFactory.Persistence;

namespace AppFactory.Mobile.Tests;

public sealed class LocalDatabaseProductionTests
{
    [Fact]
    public void PersistenceProject_UsesSupportedSQLitePackageAndIsReferencedByMobile()
    {
        var root = GetRepoRoot();
        var persistenceProject = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Persistence", "AppFactory.Persistence.csproj"));
        var mobileProject = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Mobile", "AppFactory.Mobile.csproj"));
        var testProject = File.ReadAllText(Path.Combine(root, "tests", "AppFactory.Mobile.Tests", "AppFactory.Mobile.Tests.csproj"));

        Assert.Contains("sqlite-net-pcl", persistenceProject, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("AppFactory.Persistence.csproj", mobileProject, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("AppFactory.Persistence.csproj", testProject, StringComparison.OrdinalIgnoreCase);
        Assert.True(AppDatabase.CurrentSchemaVersion > 0);
        Assert.True(AppDatabase.MaxHistoryEntries >= 100);
    }

    [Fact]
    public void GrowingCollections_UseSQLiteWhileSmallSettingsRemainPreferences()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var history = File.ReadAllText(Path.Combine(mobile, "Services", "HistoryService.cs"));
        var favorites = File.ReadAllText(Path.Combine(mobile, "Services", "FavoritesService.cs"));
        var database = File.ReadAllText(Path.Combine(root, "src", "AppFactory.Persistence", "AppDatabase.cs"));
        var tools = File.ReadAllText(Path.Combine(mobile, "Services", "ProjectToolStateService.cs"));

        Assert.Contains("Database.AddHistoryAsync", history);
        Assert.Contains("Database.GetHistoryAsync", history);
        Assert.Contains("Database.AddFavoriteAsync", favorites);
        Assert.Contains("Database.GetFavoritesAsync", favorites);
        Assert.Contains("CreateTableAsync<HistoryRecord>", database);
        Assert.Contains("CreateTableAsync<FavoriteRecord>", database);
        Assert.Contains("Preferences.Default", tools);
    }

    [Fact]
    public void LegacyPreferencesMigration_IsImplementedForHistoryAndFavorites()
    {
        var root = GetRepoRoot();
        var services = Path.Combine(root, "src", "AppFactory.Mobile", "Services");
        var history = File.ReadAllText(Path.Combine(services, "HistoryService.cs"));
        var favorites = File.ReadAllText(Path.Combine(services, "FavoritesService.cs"));

        Assert.Contains("LegacyStorageKey", history);
        Assert.Contains("MigrationFlagKey", history);
        Assert.Contains("JsonSerializer.Deserialize", history);
        Assert.Contains("LegacyStorageKey", favorites);
        Assert.Contains("MigrationFlagKey", favorites);
        Assert.Contains("JsonSerializer.Deserialize", favorites);
    }

    [Fact]
    public void Database_IsRegisteredAndHealthIsVisibleInSettings()
    {
        var root = GetRepoRoot();
        var mobile = Path.Combine(root, "src", "AppFactory.Mobile");
        var program = File.ReadAllText(Path.Combine(mobile, "MauiProgram.cs"));
        var settings = File.ReadAllText(Path.Combine(mobile, "Pages", "Settings.razor"));

        Assert.Contains("AddSingleton<LocalDatabaseService>", program);
        Assert.Contains("SQLite na urządzeniu", settings);
        Assert.Contains("Database.GetHealthAsync", settings);
        Assert.Contains("SchemaVersion", settings);
        Assert.Contains("HistoryCount", settings);
        Assert.Contains("FavoritesCount", settings);
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

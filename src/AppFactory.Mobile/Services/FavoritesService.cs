using System.Text.Json;
using AppFactory.Mobile.Models;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class FavoritesService
{
    private const string StorageKey = "appfactory:favorites:v2";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task AddAsync(FavoriteEntry entry)
    {
        var entries = Load().ToList();
        if (entries.Any(x =>
                string.Equals(x.ProjectId, entry.ProjectId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.ResultId, entry.ResultId, StringComparison.OrdinalIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        entries.Insert(0, entry);
        Save(entries);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string entryId)
    {
        var entries = Load().Where(x => !string.Equals(x.Id, entryId, StringComparison.OrdinalIgnoreCase)).ToArray();
        Save(entries);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<FavoriteEntry>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<FavoriteEntry>>(Load());

    public Task ClearAsync()
    {
        Preferences.Default.Remove(StorageKey);
        return Task.CompletedTask;
    }

    private static IReadOnlyList<FavoriteEntry> Load()
    {
        var json = Preferences.Default.Get(StorageKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<FavoriteEntry>();
        }

        try
        {
            var entries = JsonSerializer.Deserialize<List<FavoriteEntry>>(json, JsonOptions);
            return entries is null ? Array.Empty<FavoriteEntry>() : entries;
        }
        catch (JsonException)
        {
            Preferences.Default.Remove(StorageKey);
            return Array.Empty<FavoriteEntry>();
        }
    }

    private static void Save(IEnumerable<FavoriteEntry> entries)
    {
        Preferences.Default.Set(StorageKey, JsonSerializer.Serialize(entries, JsonOptions));
    }
}

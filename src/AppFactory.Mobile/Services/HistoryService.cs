using System.Text.Json;
using AppFactory.Mobile.Models;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class HistoryService
{
    private const string StorageKey = "appfactory:history:v2";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task AddAsync(HistoryEntry entry)
    {
        var entries = Load().ToList();
        entries.RemoveAll(x =>
            string.Equals(x.ProjectId, entry.ProjectId, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.CategoryId, entry.CategoryId, StringComparison.OrdinalIgnoreCase)
            && string.Equals(x.ResultId, entry.ResultId, StringComparison.OrdinalIgnoreCase));
        entries.Insert(0, entry);
        Save(entries.Take(100));
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<HistoryEntry>> GetAllAsync() =>
        Task.FromResult<IReadOnlyList<HistoryEntry>>(Load());

    public Task ClearAsync()
    {
        Preferences.Default.Remove(StorageKey);
        return Task.CompletedTask;
    }

    private static IReadOnlyList<HistoryEntry> Load()
    {
        var json = Preferences.Default.Get(StorageKey, string.Empty);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<HistoryEntry>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<HistoryEntry>>(json, JsonOptions)
                   ?? Array.Empty<HistoryEntry>();
        }
        catch (JsonException)
        {
            Preferences.Default.Remove(StorageKey);
            return Array.Empty<HistoryEntry>();
        }
    }

    private static void Save(IEnumerable<HistoryEntry> entries)
    {
        Preferences.Default.Set(StorageKey, JsonSerializer.Serialize(entries, JsonOptions));
    }
}

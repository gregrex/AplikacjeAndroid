using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class HistoryService
{
    private readonly List<HistoryEntry> _entries = new();

    public Task AddAsync(HistoryEntry entry)
    {
        _entries.Insert(0, entry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<HistoryEntry>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<HistoryEntry>>(_entries);
    }
}

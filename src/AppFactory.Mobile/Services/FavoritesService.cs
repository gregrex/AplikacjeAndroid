using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class FavoritesService
{
    private readonly List<FavoriteEntry> _entries = new();

    public Task AddAsync(FavoriteEntry entry)
    {
        if (_entries.Any(x => x.ProjectId == entry.ProjectId && x.ResultId == entry.ResultId))
        {
            return Task.CompletedTask;
        }

        _entries.Insert(0, entry);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<FavoriteEntry>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<FavoriteEntry>>(_entries);
    }
}

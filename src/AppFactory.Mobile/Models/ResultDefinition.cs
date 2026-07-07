namespace AppFactory.Mobile.Models;

public sealed class ResultDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> Needed { get; set; } = new();
    public List<string> DontDo { get; set; } = new();
}

public sealed class HistoryEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ProjectId { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string ResultId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class FavoriteEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ProjectId { get; set; } = string.Empty;
    public string ResultId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

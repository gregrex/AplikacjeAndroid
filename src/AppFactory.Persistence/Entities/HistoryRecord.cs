using SQLite;

namespace AppFactory.Persistence.Entities;

[Table("history")]
public sealed class HistoryRecord
{
    [PrimaryKey]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Indexed]
    [MaxLength(80)]
    public string ProjectId { get; set; } = string.Empty;

    [MaxLength(80)]
    public string CategoryId { get; set; } = string.Empty;

    [Indexed]
    [MaxLength(120)]
    public string ResultId { get; set; } = string.Empty;

    [MaxLength(120)]
    public string FreeResultId { get; set; } = string.Empty;

    [MaxLength(120)]
    public string PremiumResultId { get; set; } = string.Empty;

    [Indexed]
    public long CreatedAtUtcTicks { get; set; }
}

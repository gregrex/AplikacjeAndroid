using SQLite;

namespace AppFactory.Persistence.Entities;

[Table("schema_info")]
public sealed class SchemaVersionRecord
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public int Version { get; set; }

    public long UpdatedAtUtcTicks { get; set; }
}

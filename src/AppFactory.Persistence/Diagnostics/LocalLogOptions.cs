using Microsoft.Extensions.Logging;

namespace AppFactory.Persistence.Diagnostics;

public sealed class LocalLogOptions
{
    public LogLevel MinimumLevel { get; init; } = LogLevel.Information;
    public int RetentionDays { get; init; } = 7;
    public int MaxFiles { get; init; } = 12;
    public long MaxFileSizeBytes { get; init; } = 2 * 1024 * 1024;
    public int MaxMessageLength { get; init; } = 16_000;
    public int MaxExceptionLength { get; init; } = 32_000;
}

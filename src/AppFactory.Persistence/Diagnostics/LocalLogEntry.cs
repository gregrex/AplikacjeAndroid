namespace AppFactory.Persistence.Diagnostics;

public sealed class LocalLogEntry
{
    public DateTime TimestampUtc { get; init; }
    public string SessionId { get; init; } = string.Empty;
    public string Level { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int EventId { get; init; }
    public string EventName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string ExceptionType { get; init; } = string.Empty;
    public string ExceptionMessage { get; init; } = string.Empty;
    public string ExceptionStackTrace { get; init; } = string.Empty;
}

public sealed class LocalLogFileInfo
{
    public string Path { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public DateTime LastWriteTimeUtc { get; init; }
}

public sealed class LocalLogSnapshot
{
    public string SessionId { get; init; } = string.Empty;
    public string RootDirectory { get; init; } = string.Empty;
    public int FileCount { get; init; }
    public long TotalSizeBytes { get; init; }
    public DateTime? OldestLogUtc { get; init; }
    public DateTime? NewestLogUtc { get; init; }
}

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AppFactory.Persistence.Diagnostics;

public sealed class LocalLogStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private static readonly UTF8Encoding Utf8WithoutBom = new(false);

    private readonly object _sync = new();

    public LocalLogStore(string rootDirectory, LocalLogOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(rootDirectory))
        {
            throw new ArgumentException("Log directory is required.", nameof(rootDirectory));
        }

        RootDirectory = rootDirectory;
        Options = options ?? new LocalLogOptions();
        SessionId = Guid.NewGuid().ToString("N");
        Directory.CreateDirectory(RootDirectory);
        Cleanup();
    }

    public string RootDirectory { get; }
    public string SessionId { get; }
    public LocalLogOptions Options { get; }

    public bool IsEnabled(LogLevel level) => level != LogLevel.None && level >= Options.MinimumLevel;

    public void Write(LogLevel level, string category, EventId eventId, string message, Exception? exception = null)
    {
        if (!IsEnabled(level))
        {
            return;
        }

        try
        {
            var entry = new LocalLogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                SessionId = SessionId,
                Level = level.ToString(),
                Category = LogSanitizer.Sanitize(category, 512),
                EventId = eventId.Id,
                EventName = LogSanitizer.Sanitize(eventId.Name, 256),
                Message = LogSanitizer.Sanitize(message, Options.MaxMessageLength),
                ExceptionType = exception?.GetType().FullName ?? string.Empty,
                ExceptionMessage = LogSanitizer.Sanitize(exception?.Message, Options.MaxMessageLength),
                ExceptionStackTrace = LogSanitizer.Sanitize(exception?.ToString(), Options.MaxExceptionLength)
            };

            var line = JsonSerializer.Serialize(entry, JsonOptions) + Environment.NewLine;

            lock (_sync)
            {
                Directory.CreateDirectory(RootDirectory);
                var path = GetWritableFilePathUnsafe(entry.TimestampUtc);
                File.AppendAllText(path, line, Utf8WithoutBom);
                CleanupUnsafe();
            }
        }
        catch
        {
            // Logging must never crash the application or replace the original exception.
        }
    }

    public IReadOnlyList<LocalLogFileInfo> GetFiles()
    {
        lock (_sync)
        {
            return GetFileInfosUnsafe();
        }
    }

    public IReadOnlyList<string> ReadTail(int maxLines = 200)
    {
        if (maxLines <= 0)
        {
            return Array.Empty<string>();
        }

        lock (_sync)
        {
            var result = new List<string>(maxLines);
            var files = GetFileInfosUnsafe().OrderByDescending(x => x.LastWriteTimeUtc);

            foreach (var file in files)
            {
                var lines = File.ReadLines(file.Path).Reverse();
                foreach (var line in lines)
                {
                    result.Add(line);
                    if (result.Count >= maxLines)
                    {
                        result.Reverse();
                        return result;
                    }
                }
            }

            result.Reverse();
            return result;
        }
    }

    public LocalLogSnapshot GetSnapshot()
    {
        lock (_sync)
        {
            var files = GetFileInfosUnsafe();
            return new LocalLogSnapshot
            {
                SessionId = SessionId,
                RootDirectory = RootDirectory,
                FileCount = files.Count,
                TotalSizeBytes = files.Sum(x => x.SizeBytes),
                OldestLogUtc = files.Count == 0 ? null : files.Min(x => x.LastWriteTimeUtc),
                NewestLogUtc = files.Count == 0 ? null : files.Max(x => x.LastWriteTimeUtc)
            };
        }
    }

    public void Clear()
    {
        lock (_sync)
        {
            if (!Directory.Exists(RootDirectory))
            {
                return;
            }

            foreach (var path in Directory.EnumerateFiles(RootDirectory, "*.jsonl", SearchOption.TopDirectoryOnly))
            {
                TryDelete(path);
            }
        }
    }

    public void Cleanup()
    {
        lock (_sync)
        {
            CleanupUnsafe();
        }
    }

    private string GetWritableFilePathUnsafe(DateTime timestampUtc)
    {
        var prefix = $"app-{timestampUtc:yyyyMMdd}";
        for (var index = 0; index < 100; index++)
        {
            var path = Path.Combine(RootDirectory, $"{prefix}-{index:00}.jsonl");
            if (!File.Exists(path) || new FileInfo(path).Length < Options.MaxFileSizeBytes)
            {
                return path;
            }
        }

        return Path.Combine(RootDirectory, $"{prefix}-overflow.jsonl");
    }

    private void CleanupUnsafe()
    {
        if (!Directory.Exists(RootDirectory))
        {
            return;
        }

        var cutoff = DateTime.UtcNow.AddDays(-Math.Max(1, Options.RetentionDays));
        var files = GetFileInfosUnsafe().OrderByDescending(x => x.LastWriteTimeUtc).ToArray();

        foreach (var file in files.Where(x => x.LastWriteTimeUtc < cutoff))
        {
            TryDelete(file.Path);
        }

        files = GetFileInfosUnsafe().OrderByDescending(x => x.LastWriteTimeUtc).ToArray();
        foreach (var file in files.Skip(Math.Max(1, Options.MaxFiles)))
        {
            TryDelete(file.Path);
        }
    }

    private List<LocalLogFileInfo> GetFileInfosUnsafe()
    {
        if (!Directory.Exists(RootDirectory))
        {
            return new List<LocalLogFileInfo>();
        }

        return Directory.EnumerateFiles(RootDirectory, "*.jsonl", SearchOption.TopDirectoryOnly)
            .Select(path => new FileInfo(path))
            .OrderByDescending(info => info.LastWriteTimeUtc)
            .Select(info => new LocalLogFileInfo
            {
                Path = info.FullName,
                Name = info.Name,
                SizeBytes = info.Length,
                LastWriteTimeUtc = info.LastWriteTimeUtc
            })
            .ToList();
    }

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch
        {
            // Locked or inaccessible log files will be retried during the next cleanup.
        }
    }
}

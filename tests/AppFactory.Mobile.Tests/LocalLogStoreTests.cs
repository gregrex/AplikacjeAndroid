using AppFactory.Persistence.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Tests;

public sealed class LocalLogStoreTests
{
    [Fact]
    public void Write_CreatesStructuredJsonlAndRedactsSensitiveValues()
    {
        var directory = CreateDirectory();
        try
        {
            var store = new LocalLogStore(directory, new LocalLogOptions
            {
                MinimumLevel = LogLevel.Debug,
                MaxFiles = 5,
                MaxFileSizeBytes = 1024 * 1024
            });

            store.Write(
                LogLevel.Error,
                "Test.Category",
                new EventId(42, "SensitiveEvent"),
                "User test@example.com token=abc123 Authorization=secret Bearer xyz.123",
                new InvalidOperationException("password=hunter2"));

            var files = store.GetFiles();
            Assert.Single(files);
            var content = File.ReadAllText(files[0].Path);

            Assert.Contains("SensitiveEvent", content);
            Assert.Contains("[REDACTED_EMAIL]", content);
            Assert.Contains("[REDACTED]", content);
            Assert.DoesNotContain("test@example.com", content);
            Assert.DoesNotContain("abc123", content);
            Assert.DoesNotContain("hunter2", content);
            Assert.DoesNotContain("xyz.123", content);
        }
        finally
        {
            DeleteDirectory(directory);
        }
    }

    [Fact]
    public void Write_RotatesAndLimitsNumberOfFiles()
    {
        var directory = CreateDirectory();
        try
        {
            var store = new LocalLogStore(directory, new LocalLogOptions
            {
                MinimumLevel = LogLevel.Information,
                MaxFiles = 2,
                MaxFileSizeBytes = 1,
                RetentionDays = 7
            });

            for (var index = 0; index < 6; index++)
            {
                store.Write(LogLevel.Information, "Rotation", new EventId(index), $"entry-{index}");
            }

            var snapshot = store.GetSnapshot();
            Assert.InRange(snapshot.FileCount, 1, 2);
            Assert.True(snapshot.TotalSizeBytes > 0);
        }
        finally
        {
            DeleteDirectory(directory);
        }
    }

    [Fact]
    public void ReadTail_ReturnsNewestRequestedLinesInChronologicalOrder()
    {
        var directory = CreateDirectory();
        try
        {
            var store = new LocalLogStore(directory, new LocalLogOptions
            {
                MinimumLevel = LogLevel.Information,
                MaxFileSizeBytes = 1024 * 1024
            });

            store.Write(LogLevel.Information, "Tail", new EventId(1), "first");
            store.Write(LogLevel.Information, "Tail", new EventId(2), "second");
            store.Write(LogLevel.Information, "Tail", new EventId(3), "third");

            var tail = store.ReadTail(2);

            Assert.Equal(2, tail.Count);
            Assert.Contains("second", tail[0]);
            Assert.Contains("third", tail[1]);
        }
        finally
        {
            DeleteDirectory(directory);
        }
    }

    [Fact]
    public void Clear_RemovesJsonlFiles()
    {
        var directory = CreateDirectory();
        try
        {
            var store = new LocalLogStore(directory);
            store.Write(LogLevel.Information, "Clear", new EventId(1), "before-clear");
            Assert.NotEmpty(store.GetFiles());

            store.Clear();

            Assert.Empty(store.GetFiles());
        }
        finally
        {
            DeleteDirectory(directory);
        }
    }

    private static string CreateDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"appfactory-log-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
}

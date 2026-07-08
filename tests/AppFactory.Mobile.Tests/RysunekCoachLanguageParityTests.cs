using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Tests;

public sealed class RysunekCoachLanguageParityTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public void Results_AllLanguages_HaveSameResultIds()
    {
        var dataDir = GetDataDir();
        var pl = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.pl.json"));
        var en = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.en.json"));
        var uk = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, "results.uk.json"));

        Assert.Equal(pl.Select(x => x.Id).OrderBy(x => x), en.Select(x => x.Id).OrderBy(x => x));
        Assert.Equal(pl.Select(x => x.Id).OrderBy(x => x), uk.Select(x => x.Id).OrderBy(x => x));
    }

    [Fact]
    public void Results_AllLanguages_HaveRequiredContent()
    {
        var dataDir = GetDataDir();
        foreach (var file in new[] { "results.pl.json", "results.en.json", "results.uk.json" })
        {
            var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, file));
            Assert.NotEmpty(results);
            Assert.All(results, result =>
            {
                Assert.False(string.IsNullOrWhiteSpace(result.Id), $"{file}: missing id");
                Assert.False(string.IsNullOrWhiteSpace(result.Title), $"{file}: missing title for {result.Id}");
                Assert.False(string.IsNullOrWhiteSpace(result.Summary), $"{file}: missing summary for {result.Id}");
                Assert.NotEmpty(result.Steps);
            });
        }
    }

    private static T ReadJson<T>(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? throw new InvalidOperationException($"Cannot read {path}");
    }

    private static string GetDataDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "projects", "rysunek-coach", "data");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("rysunek-coach data directory not found.");
    }
}

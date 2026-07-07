using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Tests;

public sealed class PlamaRatownikLanguageParityTests
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

        var plIds = pl.Select(x => x.Id).OrderBy(x => x).ToList();
        var enIds = en.Select(x => x.Id).OrderBy(x => x).ToList();
        var ukIds = uk.Select(x => x.Id).OrderBy(x => x).ToList();

        Assert.Equal(plIds, enIds);
        Assert.Equal(plIds, ukIds);
    }

    [Fact]
    public void Results_AllLanguages_HaveNonEmptyContent()
    {
        var dataDir = GetDataDir();
        var files = new[] { "results.pl.json", "results.en.json", "results.uk.json" };

        foreach (var file in files)
        {
            var results = ReadJson<List<ResultDefinition>>(Path.Combine(dataDir, file));
            Assert.NotEmpty(results);

            foreach (var result in results)
            {
                Assert.False(string.IsNullOrWhiteSpace(result.Id), $"{file}: result id is empty.");
                Assert.False(string.IsNullOrWhiteSpace(result.Title), $"{file}: title is empty for {result.Id}.");
                Assert.False(string.IsNullOrWhiteSpace(result.Summary), $"{file}: summary is empty for {result.Id}.");
                Assert.NotEmpty(result.Steps);
            }
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
            var candidate = Path.Combine(dir.FullName, "src", "AppFactory.Mobile", "wwwroot", "projects", "plama-ratownik");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("plama-ratownik data directory not found.");
    }
}

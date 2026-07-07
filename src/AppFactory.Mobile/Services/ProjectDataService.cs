using System.Text.Json;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ProjectDataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<AppConfig?> LoadAppConfigAsync(string projectId)
    {
        return await LoadJsonAsync<AppConfig>($"projects/{projectId}/app.json");
    }

    public async Task<List<CategoryDefinition>> LoadCategoriesAsync(string projectId)
    {
        return await LoadJsonAsync<List<CategoryDefinition>>($"projects/{projectId}/categories.json") ?? new();
    }

    public async Task<List<QuestionDefinition>> LoadQuestionsAsync(string projectId)
    {
        return await LoadJsonAsync<List<QuestionDefinition>>($"projects/{projectId}/questions.json") ?? new();
    }

    public async Task<List<RuleDefinition>> LoadRulesAsync(string projectId)
    {
        return await LoadJsonAsync<List<RuleDefinition>>($"projects/{projectId}/rules.json") ?? new();
    }

    public async Task<List<ResultDefinition>> LoadResultsAsync(string projectId, string language = "pl")
    {
        return await LoadJsonAsync<List<ResultDefinition>>($"projects/{projectId}/results.{language}.json") ?? new();
    }

    private static async Task<T?> LoadJsonAsync<T>(string relativePath)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(relativePath);
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions);
        }
        catch
        {
            return default;
        }
    }
}

using System.Text.Json;
using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class ProjectDataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ILogger<ProjectDataService> _logger;

    public ProjectDataService(ILogger<ProjectDataService> logger)
    {
        _logger = logger;
    }

    public Task<AppConfig?> LoadAppConfigAsync(string projectId) =>
        LoadJsonAsync<AppConfig>($"projects/{projectId}/app.json");

    public async Task<List<CategoryDefinition>> LoadCategoriesAsync(string projectId) =>
        await LoadJsonAsync<List<CategoryDefinition>>($"projects/{projectId}/categories.json") ?? new();

    public async Task<List<QuestionDefinition>> LoadQuestionsAsync(string projectId) =>
        await LoadJsonAsync<List<QuestionDefinition>>($"projects/{projectId}/questions.json") ?? new();

    public async Task<List<RuleDefinition>> LoadRulesAsync(string projectId) =>
        await LoadJsonAsync<List<RuleDefinition>>($"projects/{projectId}/rules.json") ?? new();

    public async Task<List<ResultDefinition>> LoadResultsAsync(string projectId, string language = "pl") =>
        await LoadJsonAsync<List<ResultDefinition>>($"projects/{projectId}/results.{language}.json") ?? new();

    private async Task<T?> LoadJsonAsync<T>(string relativePath)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(relativePath);
            var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions);
            if (result is null)
            {
                _logger.LogWarning("Project data deserialized to null. Path={RelativePath} Type={Type}", relativePath, typeof(T).Name);
            }
            else
            {
                _logger.LogDebug("Project data loaded. Path={RelativePath} Type={Type}", relativePath, typeof(T).Name);
            }

            return result;
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, "Project data file was not found. Path={RelativePath}", relativePath);
            return default;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Project data JSON is invalid. Path={RelativePath} Type={Type}", relativePath, typeof(T).Name);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Project data loading failed. Path={RelativePath} Type={Type}", relativePath, typeof(T).Name);
            return default;
        }
    }
}

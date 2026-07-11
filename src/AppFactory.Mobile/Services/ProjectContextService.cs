using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class ProjectContextService
{
    private readonly ILogger<ProjectContextService> _logger;

    public ProjectContextService(ILogger<ProjectContextService> logger)
    {
        _logger = logger;
    }

    public event Action<string>? ProjectChanged;

    public string CurrentProjectId { get; private set; } = "plama-ratownik";

    public void SelectProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            _logger.LogWarning("Project selection ignored because project id was empty.");
            return;
        }

        if (string.Equals(CurrentProjectId, projectId, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("Project selection unchanged. Project={ProjectId}", projectId);
            return;
        }

        var previousProjectId = CurrentProjectId;
        CurrentProjectId = projectId;
        _logger.LogInformation(
            "Project context changed. PreviousProject={PreviousProjectId} CurrentProject={CurrentProjectId}",
            previousProjectId,
            CurrentProjectId);
        ProjectChanged?.Invoke(CurrentProjectId);
    }
}

namespace AppFactory.Mobile.Services;

public sealed class ProjectContextService
{
    public event Action<string>? ProjectChanged;

    public string CurrentProjectId { get; private set; } = "plama-ratownik";

    public void SelectProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            return;
        }

        if (string.Equals(CurrentProjectId, projectId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        CurrentProjectId = projectId;
        ProjectChanged?.Invoke(CurrentProjectId);
    }
}

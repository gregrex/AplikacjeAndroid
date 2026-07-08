namespace AppFactory.Mobile.Services;

public sealed class ProjectContextService
{
    public string CurrentProjectId { get; private set; } = "plama-ratownik";

    public void SelectProject(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            return;
        }

        CurrentProjectId = projectId;
    }
}

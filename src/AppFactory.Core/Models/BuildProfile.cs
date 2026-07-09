namespace AppFactory.Mobile.Models;

public sealed class BuildProfile
{
    public string ProjectId { get; init; } = string.Empty;
    public string ApplicationTitle { get; init; } = string.Empty;
    public string ApplicationId { get; init; } = string.Empty;
    public string DisplayVersion { get; init; } = "0.1.0";
    public int Version { get; init; } = 1;
    public bool IsCatalogBuild { get; init; }
}

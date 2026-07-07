namespace AppFactory.Mobile.Models;

public sealed class AppConfig
{
    public string AppId { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public string DefaultLanguage { get; set; } = "pl";
    public List<string> SupportedLanguages { get; set; } = new();
    public ThemeConfig Theme { get; set; } = new();
    public FeatureConfig Features { get; set; } = new();
}

public sealed class ThemeConfig
{
    public string PrimaryColor { get; set; } = "#2563EB";
    public string SecondaryColor { get; set; } = "#F97316";
}

public sealed class FeatureConfig
{
    public bool History { get; set; } = true;
    public bool Favorites { get; set; } = true;
    public bool PhotoOptional { get; set; }
    public bool ExportText { get; set; } = true;
}

namespace AppFactory.Mobile.Models;

public sealed class ThemeDefinition
{
    public string ThemeId { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string TargetAudience { get; set; } = string.Empty;
    public string Tone { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#2563EB";
    public string SecondaryColor { get; set; } = "#F97316";
    public string AccentColor { get; set; } = "#22C55E";
    public string BackgroundColor { get; set; } = "#F8FAFC";
    public string SurfaceColor { get; set; } = "#FFFFFF";
    public string TextColor { get; set; } = "#0F172A";
    public string MutedTextColor { get; set; } = "#64748B";
    public string DangerColor { get; set; } = "#DC2626";
    public string BackgroundStyle { get; set; } = "light-clean";
    public string CardStyle { get; set; } = "rounded-soft";
    public string ButtonStyle { get; set; } = "solid-rounded";
    public string IconStyle { get; set; } = "outline-minimal";
    public string IllustrationStyle { get; set; } = "simple-flat";
    public string ResultScreenStyle { get; set; } = "instruction-card";
    public string OnboardingStyle { get; set; } = "short-practical";
    public string VisualMood { get; set; } = string.Empty;
}

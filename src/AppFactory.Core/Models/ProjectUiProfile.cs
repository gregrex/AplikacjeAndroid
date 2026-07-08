namespace AppFactory.Mobile.Models;

public sealed class ProjectUiProfile
{
    public string ProjectId { get; init; } = string.Empty;
    public string ResultViewType { get; init; } = "instruction-checklist";
    public string PrimaryActionLabel { get; init; } = "Odblokuj pełną wersję";
    public string SecondaryActionLabel { get; init; } = "Dodaj do ulubionych";
    public bool ShowCopyAction { get; init; }
    public bool ShowSafetyBlock { get; init; }
    public bool ShowNeededBlock { get; init; } = true;
    public bool ShowDontDoBlock { get; init; } = true;
    public string EmptyStateText { get; init; } = "Nie znaleziono wyniku.";
}

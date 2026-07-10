namespace AppFactory.Mobile.Models;

public sealed class ProjectUiProfile
{
    public string ProjectId { get; init; } = string.Empty;
    public string Icon { get; init; } = "✨";
    public string Badge { get; init; } = "Praktyczny pomocnik";
    public string HeroTitle { get; init; } = "Szybka pomoc krok po kroku";
    public string HeroDescription { get; init; } = "Odpowiedz na kilka pytań i otrzymaj dopasowaną rekomendację.";
    public string CategoryHeading { get; init; } = "Wybierz temat";
    public string QuizHeading { get; init; } = "Dopasuj rekomendację";
    public string ResultHeading { get; init; } = "Twoja rekomendacja";
    public string ResultViewType { get; init; } = "instruction-checklist";
    public string PrimaryActionLabel { get; init; } = "Odblokuj pełną wersję";
    public string SecondaryActionLabel { get; init; } = "Dodaj do ulubionych";
    public string ToolKind { get; init; } = string.Empty;
    public bool ShowCopyAction { get; init; }
    public bool ShowSafetyBlock { get; init; }
    public bool ShowNeededBlock { get; init; } = true;
    public bool ShowDontDoBlock { get; init; } = true;
    public string EmptyStateText { get; init; } = "Nie znaleziono wyniku. Wróć do quizu i spróbuj ponownie.";
}

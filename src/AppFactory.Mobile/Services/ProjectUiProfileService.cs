using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ProjectUiProfileService
{
    private static readonly ProjectUiProfile DefaultProfile = new()
    {
        ResultViewType = "instruction-checklist",
        PrimaryActionLabel = "Odblokuj pełną instrukcję",
        SecondaryActionLabel = "Dodaj do ulubionych",
        ShowSafetyBlock = true
    };

    public ProjectUiProfile GetProfile(string projectId)
    {
        return projectId switch
        {
            "plama-ratownik" => new ProjectUiProfile
            {
                ProjectId = projectId,
                ResultViewType = "instruction-checklist",
                PrimaryActionLabel = "Odblokuj pełną instrukcję",
                SecondaryActionLabel = "Zapisz instrukcję",
                ShowSafetyBlock = true
            },
            "kolek-dobieracz" => new ProjectUiProfile
            {
                ProjectId = projectId,
                ResultViewType = "technical-safety-checklist",
                PrimaryActionLabel = "Pokaż pełną checklistę montażu",
                SecondaryActionLabel = "Zapisz mocowanie",
                ShowSafetyBlock = true
            },
            "pies-trener-7dni" => new ProjectUiProfile
            {
                ProjectId = projectId,
                ResultViewType = "seven-day-plan",
                PrimaryActionLabel = "Pokaż pełny plan 7 dni",
                SecondaryActionLabel = "Zapisz plan",
                ShowSafetyBlock = true
            },
            "bajka-z-rysunku" => new ProjectUiProfile
            {
                ProjectId = projectId,
                ResultViewType = "story-page",
                PrimaryActionLabel = "Pokaż pełną bajkę",
                SecondaryActionLabel = "Zapisz bajkę",
                ShowSafetyBlock = true
            },
            "vinted-olx-opis" => new ProjectUiProfile
            {
                ProjectId = projectId,
                ResultViewType = "sales-copy-card",
                PrimaryActionLabel = "Pokaż pełny opis",
                SecondaryActionLabel = "Zapisz szablon",
                ShowCopyAction = true,
                ShowSafetyBlock = true
            },
            _ => DefaultProfile
        };
    }
}

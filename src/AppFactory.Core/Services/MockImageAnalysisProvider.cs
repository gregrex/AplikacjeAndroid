using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class MockImageAnalysisProvider : IImageAnalysisProvider
{
    public Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        var suggestions = GetSuggestions(request.ProjectId);
        var warnings = GetWarnings(request.ProjectId, policy.IsSafetySensitive);

        return Task.FromResult(new ImageAnalysisResult
        {
            ProjectId = request.ProjectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = true,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = GetSummary(request.ProjectId),
            SuggestedAnswers = suggestions,
            Warnings = warnings
        });
    }

    private static IReadOnlyList<ImageAnswerSuggestion> GetSuggestions(string projectId) =>
        projectId switch
        {
            "plama-ratownik" => new[]
            {
                Suggest("material", "cotton", 0.62, "Mock rozpoznał jasną tkaninę przypominającą bawełnę."),
                Suggest("fresh", "yes", 0.58, "Mock zakłada świeżą plamę, bo zabrudzenie wygląda na mokre.")
            },
            "pokoj-makeover" => new[]
            {
                Suggest("budget", "zero", 0.7, "Mock sugeruje metamorfozę bez zakupów na podstawie układu pokoju."),
                Suggest("style", "cozy", 0.54, "Mock wykrył potencjał na przytulny efekt przez światło i tekstylia.")
            },
            "rysunek-coach" => new[]
            {
                Suggest("topic", "animal", 0.6, "Mock wykrył szkic przypominający proste zwierzę.")
            },
            "outfit-coach" => new[]
            {
                Suggest("style", "smart", 0.61, "Mock wykrył elementy pasujące do stylu smart casual.")
            },
            "fryzury-proste" => new[]
            {
                Suggest("finish", "natural", 0.57, "Mock sugeruje naturalne wykończenie fryzury.")
            },
            "barber-translator" => new[]
            {
                Suggest("style", "natural", 0.59, "Mock sugeruje naturalny opis fryzury do pokazania barberowi.")
            },
            "zmywarka-diagnosta" => new[]
            {
                Suggest("problem", "residue", 0.55, "Mock wykrył ślad przypominający osad lub nalot.")
            },
            "silikon-fuga-fix" => new[]
            {
                Suggest("problem", "mold", 0.55, "Mock wykrył ciemny obszar wymagający ręcznej weryfikacji.")
            },
            _ => Array.Empty<ImageAnswerSuggestion>()
        };

    private static IReadOnlyList<string> GetWarnings(string projectId, bool isSafetySensitive)
    {
        var warnings = new List<string>
        {
            "Analiza obrazu jest podpowiedzią. Użytkownik powinien potwierdzić odpowiedzi ręcznie."
        };

        if (isSafetySensitive)
        {
            warnings.Add("Projekt jest safety-sensitive: zdjęcie nie zastępuje oceny ryzyka ani pomocy fachowca.");
        }

        if (string.Equals(projectId, "silikon-fuga-fix", StringComparison.OrdinalIgnoreCase))
        {
            warnings.Add("Przy podejrzeniu pleśni użyj rękawic, wentylacji i nie mieszaj środków chemicznych.");
        }

        if (string.Equals(projectId, "zmywarka-diagnosta", StringComparison.OrdinalIgnoreCase))
        {
            warnings.Add("Przy wycieku, zapachu spalenizny lub ryzyku porażenia odłącz urządzenie i skontaktuj się z serwisem.");
        }

        return warnings;
    }

    private static string GetSummary(string projectId) =>
        projectId switch
        {
            "plama-ratownik" => "Mock analiza obrazu: zdjęcie może pomóc w wyborze materiału, świeżości i typu plamy.",
            "pokoj-makeover" => "Mock analiza obrazu: zdjęcie może pomóc określić styl, ograniczenia i priorytet metamorfozy.",
            "rysunek-coach" => "Mock analiza obrazu: zdjęcie szkicu może podpowiedzieć temat i następne ćwiczenie.",
            "outfit-coach" => "Mock analiza obrazu: zdjęcie może podpowiedzieć styl i kontekst stroju.",
            "fryzury-proste" => "Mock analiza obrazu: zdjęcie może podpowiedzieć typ wykończenia fryzury.",
            "barber-translator" => "Mock analiza obrazu: zdjęcie może pomóc przygotować neutralny brief dla barbera.",
            "zmywarka-diagnosta" => "Mock analiza obrazu: zdjęcie może wskazać osad, filtr lub typowy objaw do ręcznej weryfikacji.",
            "silikon-fuga-fix" => "Mock analiza obrazu: zdjęcie może wskazać obszar fugi lub silikonu wymagający kontroli.",
            _ => "Mock analiza obrazu: projekt nie ma dedykowanej konfiguracji."
        };

    private static ImageAnswerSuggestion Suggest(string questionId, string value, double confidence, string reason) => new()
    {
        QuestionId = questionId,
        Value = value,
        Confidence = confidence,
        Reason = reason
    };
}

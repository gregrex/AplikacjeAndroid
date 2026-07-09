using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class MockImageAnalysisProvider : IImageAnalysisProvider
{
    public Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ImageAnalysisResult
        {
            ProjectId = request.ProjectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = true,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = GetSummary(request.ProjectId),
            SuggestedAnswers = GetSuggestions(request.ProjectId),
            Warnings = GetWarnings(policy.IsSafetySensitive)
        });
    }

    private static IReadOnlyList<ImageAnswerSuggestion> GetSuggestions(string projectId) =>
        projectId switch
        {
            "plama-ratownik" => new[] { Suggest("material", "cotton", 0.62), Suggest("fresh", "yes", 0.58) },
            "pokoj-makeover" => new[] { Suggest("budget", "zero", 0.70), Suggest("style", "cozy", 0.54) },
            "rysunek-coach" => new[] { Suggest("topic", "animal", 0.60) },
            "outfit-coach" => new[] { Suggest("style", "smart", 0.61) },
            "fryzury-proste" => new[] { Suggest("finish", "natural", 0.57) },
            "barber-translator" => new[] { Suggest("style", "natural", 0.59) },
            "zmywarka-diagnosta" => new[] { Suggest("problem", "residue", 0.55) },
            "silikon-fuga-fix" => new[] { Suggest("problem", "mold", 0.55) },
            _ => Array.Empty<ImageAnswerSuggestion>()
        };

    private static IReadOnlyList<string> GetWarnings(bool isSafetySensitive)
    {
        var warnings = new List<string>
        {
            "Analiza obrazu jest tylko podpowiedzią. Użytkownik powinien potwierdzić odpowiedzi ręcznie."
        };

        if (isSafetySensitive)
        {
            warnings.Add("Projekt wymaga ostrożności: zdjęcie nie zastępuje oceny ryzyka ani pomocy fachowca.");
        }

        return warnings;
    }

    private static string GetSummary(string projectId) =>
        projectId switch
        {
            "plama-ratownik" => "Mock analiza obrazu: zdjęcie może pomóc dobrać odpowiedzi o plamie.",
            "pokoj-makeover" => "Mock analiza obrazu: zdjęcie może pomóc określić styl i priorytet pokoju.",
            "rysunek-coach" => "Mock analiza obrazu: zdjęcie szkicu może podpowiedzieć temat ćwiczenia.",
            "outfit-coach" => "Mock analiza obrazu: zdjęcie może podpowiedzieć styl stroju.",
            "fryzury-proste" => "Mock analiza obrazu: zdjęcie może podpowiedzieć typ wykończenia fryzury.",
            "barber-translator" => "Mock analiza obrazu: zdjęcie może pomóc przygotować brief dla barbera.",
            "zmywarka-diagnosta" => "Mock analiza obrazu: zdjęcie może pomóc wskazać objaw do sprawdzenia.",
            "silikon-fuga-fix" => "Mock analiza obrazu: zdjęcie może pomóc wskazać obszar do kontroli.",
            _ => "Mock analiza obrazu: projekt nie ma dedykowanej konfiguracji."
        };

    private static ImageAnswerSuggestion Suggest(string questionId, string value, double confidence) => new()
    {
        QuestionId = questionId,
        Value = value,
        Confidence = confidence,
        Reason = "Sugestia z mock providera."
    };
}

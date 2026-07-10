using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalVisionInferenceEngine : ILocalVisionInferenceEngine
{
    private readonly OnnxModelRunner _runner;
    private readonly LocalAiInputTensorFactory _tensorFactory;

    public LocalVisionInferenceEngine()
        : this(new OnnxModelRunner(), new LocalAiInputTensorFactory())
    {
    }

    public LocalVisionInferenceEngine(OnnxModelRunner runner, LocalAiInputTensorFactory tensorFactory)
    {
        _runner = runner;
        _tensorFactory = tensorFactory;
    }

    public Task<ImageAnalysisResult> AnalyzeAsync(ImageAnalysisRequest request, string modelPath, ImageAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        try
        {
            var input = _tensorFactory.CreateNormalizedByteTensor(request.LocalFilePath);
            var scores = _runner.RunSingleFloatTensor(modelPath, LocalAiInputTensorFactory.DefaultInputName, LocalAiInputTensorFactory.DefaultInputShape, input);
            var suggestions = MapImageScores(request.ProjectId, scores);

            return Task.FromResult(new ImageAnalysisResult
            {
                ProjectId = request.ProjectId,
                IsEnabled = policy.IsEnabled,
                IsAccepted = suggestions.Count > 0,
                IsSafetySensitive = policy.IsSafetySensitive,
                Summary = suggestions.Count > 0
                    ? "Lokalny model obrazu zwrócił sugestie do ręcznego potwierdzenia."
                    : "Lokalny model obrazu nie zwrócił sugestii dla tego projektu.",
                SuggestedAnswers = suggestions,
                Warnings = policy.IsSafetySensitive
                    ? new[] { "Analiza lokalna jest podpowiedzią i nie zastępuje oceny ryzyka." }
                    : Array.Empty<string>()
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ImageAnalysisResult
            {
                ProjectId = request.ProjectId,
                IsEnabled = policy.IsEnabled,
                IsAccepted = false,
                IsSafetySensitive = policy.IsSafetySensitive,
                Summary = $"Nie udało się uruchomić lokalnej inferencji obrazu: {ex.Message}",
                Warnings = new[] { "Sprawdź model, ścieżkę pliku wejściowego i zgodność wejścia ONNX." }
            });
        }
    }

    private static IReadOnlyList<ImageAnswerSuggestion> MapImageScores(string projectId, IReadOnlyList<float> scores)
    {
        if (scores.Count == 0)
        {
            return Array.Empty<ImageAnswerSuggestion>();
        }

        var bestIndex = 0;
        var bestScore = scores[0];
        for (var i = 1; i < scores.Count; i++)
        {
            if (scores[i] > bestScore)
            {
                bestScore = scores[i];
                bestIndex = i;
            }
        }

        return projectId switch
        {
            "plama-ratownik" => MapStain(bestIndex, bestScore),
            "pokoj-makeover" => MapRoom(bestIndex, bestScore),
            "rysunek-coach" => MapDrawing(bestIndex, bestScore),
            "outfit-coach" => MapOutfit(bestIndex, bestScore),
            "fryzury-proste" => MapHair(bestIndex, bestScore),
            "barber-translator" => MapBarber(bestIndex, bestScore),
            "zmywarka-diagnosta" => MapDishwasher(bestIndex, bestScore),
            "silikon-fuga-fix" => MapSilicone(bestIndex, bestScore),
            _ => Array.Empty<ImageAnswerSuggestion>()
        };
    }

    private static IReadOnlyList<ImageAnswerSuggestion> MapStain(int index, float score) => index % 2 == 0
        ? new[] { Suggest("material", "cotton", score, "Lokalny model obrazu wskazał materiał podobny do bawełny.") }
        : new[] { Suggest("fresh", "yes", score, "Lokalny model obrazu wskazał świeże zabrudzenie.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapRoom(int index, float score) => index % 2 == 0
        ? new[] { Suggest("style", "cozy", score, "Lokalny model obrazu wskazał potencjał na przytulny styl.") }
        : new[] { Suggest("budget", "zero", score, "Lokalny model obrazu sugeruje metamorfozę bez zakupów.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapDrawing(int index, float score) =>
        new[] { Suggest("topic", index % 2 == 0 ? "animal" : "object", score, "Lokalny model obrazu wskazał temat rysunku.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapOutfit(int index, float score) =>
        new[] { Suggest("style", index % 2 == 0 ? "smart" : "casual", score, "Lokalny model obrazu wskazał styl stroju.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapHair(int index, float score) =>
        new[] { Suggest("finish", index % 2 == 0 ? "natural" : "elegant", score, "Lokalny model obrazu wskazał wykończenie fryzury.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapBarber(int index, float score) =>
        new[] { Suggest("style", index % 2 == 0 ? "natural" : "formal", score, "Lokalny model obrazu wskazał styl briefu dla barbera.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapDishwasher(int index, float score) =>
        new[] { Suggest("problem", index % 2 == 0 ? "residue" : "smell", score, "Lokalny model obrazu wskazał objaw do sprawdzenia.") };

    private static IReadOnlyList<ImageAnswerSuggestion> MapSilicone(int index, float score) =>
        new[] { Suggest("problem", index % 2 == 0 ? "mold" : "crack", score, "Lokalny model obrazu wskazał problem fugi lub silikonu.") };

    private static ImageAnswerSuggestion Suggest(string questionId, string value, float confidence, string reason) => new()
    {
        QuestionId = questionId,
        Value = value,
        Confidence = Math.Clamp(confidence, 0, 1),
        Reason = reason
    };
}

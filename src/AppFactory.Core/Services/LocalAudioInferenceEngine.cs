using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalAudioInferenceEngine : ILocalAudioInferenceEngine
{
    private readonly OnnxModelRunner _runner;
    private readonly LocalAiInputTensorFactory _tensorFactory;

    public LocalAudioInferenceEngine()
        : this(new OnnxModelRunner(), new LocalAiInputTensorFactory())
    {
    }

    public LocalAudioInferenceEngine(OnnxModelRunner runner, LocalAiInputTensorFactory tensorFactory)
    {
        _runner = runner;
        _tensorFactory = tensorFactory;
    }

    public Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, string modelPath, AudioAnalysisProjectPolicy policy, CancellationToken cancellationToken = default)
    {
        try
        {
            var input = _tensorFactory.CreateNormalizedByteTensor(request.LocalFilePath);
            var scores = _runner.RunSingleFloatTensor(modelPath, LocalAiInputTensorFactory.DefaultInputName, LocalAiInputTensorFactory.DefaultInputShape, input);
            var suggestions = MapAudioScores(request.ProjectId, scores);

            return Task.FromResult(new AudioAnalysisResult
            {
                ProjectId = request.ProjectId,
                IsEnabled = policy.IsEnabled,
                IsAccepted = suggestions.Count > 0,
                IsSafetySensitive = policy.IsSafetySensitive,
                Summary = suggestions.Count > 0
                    ? "Lokalny model dźwięku zwrócił sugestie do ręcznego potwierdzenia."
                    : "Lokalny model dźwięku nie zwrócił sugestii dla tego projektu.",
                SuggestedAnswers = suggestions,
                Warnings = policy.IsSafetySensitive
                    ? new[] { "Analiza lokalna jest podpowiedzią i nie zastępuje oceny ryzyka." }
                    : Array.Empty<string>()
            });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new AudioAnalysisResult
            {
                ProjectId = request.ProjectId,
                IsEnabled = policy.IsEnabled,
                IsAccepted = false,
                IsSafetySensitive = policy.IsSafetySensitive,
                Summary = $"Nie udało się uruchomić lokalnej inferencji dźwięku: {ex.Message}",
                Warnings = new[] { "Sprawdź model, ścieżkę pliku wejściowego i zgodność wejścia ONNX." }
            });
        }
    }

    private static IReadOnlyList<AudioAnswerSuggestion> MapAudioScores(string projectId, IReadOnlyList<float> scores)
    {
        if (scores.Count == 0)
        {
            return Array.Empty<AudioAnswerSuggestion>();
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
            "zmywarka-diagnosta" => MapDishwasherAudio(bestIndex, bestScore),
            "pies-trener-7dni" => MapDogAudio(bestIndex, bestScore),
            "kot-bawi-sie" => MapCatAudio(bestIndex, bestScore),
            _ => Array.Empty<AudioAnswerSuggestion>()
        };
    }

    private static IReadOnlyList<AudioAnswerSuggestion> MapDishwasherAudio(int index, float score) => index % 2 == 0
        ? new[] { Suggest("problem", "noise", score, "Lokalny model dźwięku wskazał nietypowy hałas.") }
        : new[] { Suggest("problem", "drain", score, "Lokalny model dźwięku wskazał możliwy problem z odpompowaniem.") };

    private static IReadOnlyList<AudioAnswerSuggestion> MapDogAudio(int index, float score) => index % 2 == 0
        ? new[] { Suggest("energy", "high", score, "Lokalny model dźwięku wskazał wysoką pobudliwość.") }
        : new[] { Suggest("level", "easy", score, "Lokalny model dźwięku sugeruje łagodny plan ćwiczeń.") };

    private static IReadOnlyList<AudioAnswerSuggestion> MapCatAudio(int index, float score) => index % 2 == 0
        ? new[] { Suggest("energy", "high", score, "Lokalny model dźwięku wskazał aktywny nastrój kota.") }
        : new[] { Suggest("energy", "low", score, "Lokalny model dźwięku wskazał spokojny nastrój kota.") };

    private static AudioAnswerSuggestion Suggest(string questionId, string value, float confidence, string reason) => new()
    {
        QuestionId = questionId,
        Value = value,
        Confidence = Math.Clamp(confidence, 0, 1),
        Reason = reason
    };
}

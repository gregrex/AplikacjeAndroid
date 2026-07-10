using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class AudioAnalysisService
{
    private readonly AudioAnalysisPolicyService _policyService;
    private readonly IAudioAnalysisProvider _provider;

    public AudioAnalysisService(AudioAnalysisPolicyService policyService, IAudioAnalysisProvider provider)
    {
        _policyService = policyService;
        _provider = provider;
    }

    public async Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, CancellationToken cancellationToken = default)
    {
        var policy = _policyService.GetPolicy(request.ProjectId);
        if (!policy.IsEnabled)
        {
            return Rejected(request.ProjectId, policy, "Ten projekt nie obsługuje analizy dźwięku.");
        }

        if (!policy.AcceptedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return Rejected(request.ProjectId, policy, $"Nieobsługiwany typ pliku audio: {request.ContentType}.");
        }

        if (request.SizeBytes <= 0)
        {
            return Rejected(request.ProjectId, policy, "Plik audio jest pusty.");
        }

        if (request.SizeBytes > policy.MaxSizeBytes)
        {
            return Rejected(request.ProjectId, policy, $"Plik audio jest za duży. Limit: {policy.MaxSizeBytes} bajtów.");
        }

        if (request.Duration <= TimeSpan.Zero)
        {
            return Rejected(request.ProjectId, policy, "Nagranie audio ma nieprawidłowy czas trwania.");
        }

        if (request.Duration > policy.MaxDuration)
        {
            return Rejected(request.ProjectId, policy, $"Nagranie audio jest za długie. Limit: {policy.MaxDuration.TotalSeconds:0} sekund.");
        }

        return await _provider.AnalyzeAsync(request, policy, cancellationToken);
    }

    private static AudioAnalysisResult Rejected(string projectId, AudioAnalysisProjectPolicy policy, string message) => new()
    {
        ProjectId = projectId,
        IsEnabled = policy.IsEnabled,
        IsAccepted = false,
        IsSafetySensitive = policy.IsSafetySensitive,
        Summary = message,
        Warnings = new[] { message }
    };
}

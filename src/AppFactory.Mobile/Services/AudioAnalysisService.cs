using System.Diagnostics;
using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile.Services;

public sealed class AudioAnalysisService
{
    private readonly AudioAnalysisPolicyService _policyService;
    private readonly IAudioAnalysisProvider _provider;
    private readonly ILogger<AudioAnalysisService> _logger;

    public AudioAnalysisService(
        AudioAnalysisPolicyService policyService,
        IAudioAnalysisProvider provider,
        ILogger<AudioAnalysisService> logger)
    {
        _policyService = policyService;
        _provider = provider;
        _logger = logger;
    }

    public async Task<AudioAnalysisResult> AnalyzeAsync(AudioAnalysisRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation(
            "Audio analysis started. Project={ProjectId} ContentType={ContentType} SizeBytes={SizeBytes} DurationSeconds={DurationSeconds}",
            request.ProjectId,
            request.ContentType,
            request.SizeBytes,
            request.Duration.TotalSeconds);

        var policy = _policyService.GetPolicy(request.ProjectId);
        if (!policy.IsEnabled)
        {
            return Reject(request.ProjectId, policy, "Ten projekt nie obsługuje analizy dźwięku.");
        }

        if (!policy.AcceptedContentTypes.Contains(request.ContentType, StringComparer.OrdinalIgnoreCase))
        {
            return Reject(request.ProjectId, policy, $"Nieobsługiwany typ pliku audio: {request.ContentType}.");
        }

        if (request.SizeBytes <= 0)
        {
            return Reject(request.ProjectId, policy, "Plik audio jest pusty.");
        }

        if (request.SizeBytes > policy.MaxSizeBytes)
        {
            return Reject(request.ProjectId, policy, $"Plik audio jest za duży. Limit: {policy.MaxSizeBytes} bajtów.");
        }

        if (request.Duration <= TimeSpan.Zero)
        {
            return Reject(request.ProjectId, policy, "Nagranie audio ma nieprawidłowy czas trwania.");
        }

        if (request.Duration > policy.MaxDuration)
        {
            return Reject(request.ProjectId, policy, $"Nagranie audio jest za długie. Limit: {policy.MaxDuration.TotalSeconds:0} sekund.");
        }

        try
        {
            var result = await _provider.AnalyzeAsync(request, policy, cancellationToken);
            stopwatch.Stop();
            _logger.LogInformation(
                "Audio analysis completed. Project={ProjectId} Accepted={Accepted} Suggestions={SuggestionCount} DurationMs={DurationMs}",
                request.ProjectId,
                result.IsAccepted,
                result.SuggestedAnswers.Count,
                stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            _logger.LogWarning(
                "Audio analysis cancelled. Project={ProjectId} DurationMs={DurationMs}",
                request.ProjectId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "Audio analysis failed unexpectedly. Project={ProjectId} DurationMs={DurationMs}",
                request.ProjectId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    private AudioAnalysisResult Reject(string projectId, AudioAnalysisProjectPolicy policy, string message)
    {
        _logger.LogWarning("Audio analysis rejected. Project={ProjectId} Reason={Reason}", projectId, message);
        return new AudioAnalysisResult
        {
            ProjectId = projectId,
            IsEnabled = policy.IsEnabled,
            IsAccepted = false,
            IsSafetySensitive = policy.IsSafetySensitive,
            Summary = message,
            Warnings = new[] { message }
        };
    }
}

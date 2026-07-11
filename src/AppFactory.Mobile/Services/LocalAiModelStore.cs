using System.Diagnostics;
using System.Security.Cryptography;
using AppFactory.Mobile.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AppFactory.Mobile.Services;

public sealed class LocalAiModelStore
{
    private readonly HttpClient _httpClient;
    private readonly string _modelDirectory;
    private readonly ILogger<LocalAiModelStore> _logger;

    public LocalAiModelStore(
        HttpClient? httpClient = null,
        string? modelDirectory = null,
        ILogger<LocalAiModelStore>? logger = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _modelDirectory = modelDirectory ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "appfactory-models");
        _logger = logger ?? NullLogger<LocalAiModelStore>.Instance;
    }

    public LocalAiModelStatus GetStatus(LocalAiModelProfile profile)
    {
        var configured = IsConfigured(profile);
        var path = GetLocalPath(profile);
        var exists = File.Exists(path);
        var verified = exists && configured && VerifySha256(path, profile.Sha256);

        return new LocalAiModelStatus
        {
            ModelId = profile.ModelId,
            IsConfigured = configured,
            IsDownloaded = exists,
            IsVerified = verified,
            LocalPath = path,
            Message = configured ? (verified ? "Model gotowy." : "Model wymaga pobrania lub weryfikacji.") : "Model nie ma skonfigurowanego URL lub SHA256."
        };
    }

    public async Task<LocalAiModelDownloadResult> DownloadAsync(LocalAiModelProfile profile, CancellationToken cancellationToken = default)
    {
        if (!IsConfigured(profile))
        {
            _logger.LogWarning("AI model download rejected because configuration is incomplete. ModelId={ModelId}", profile.ModelId);
            return new LocalAiModelDownloadResult
            {
                ModelId = profile.ModelId,
                Success = false,
                LocalPath = GetLocalPath(profile),
                Message = "Model nie ma skonfigurowanego URL lub SHA256."
            };
        }

        Directory.CreateDirectory(_modelDirectory);
        var path = GetLocalPath(profile);
        var temporaryPath = $"{path}.download";
        var stopwatch = Stopwatch.StartNew();
        var host = Uri.TryCreate(profile.DownloadUrl, UriKind.Absolute, out var uri) ? uri.Host : "unknown";

        TryDelete(temporaryPath);
        _logger.LogInformation(
            "AI model download started. ModelId={ModelId} Version={Version} Host={Host} ExpectedSizeBytes={ExpectedSizeBytes}",
            profile.ModelId,
            profile.Version,
            host,
            profile.SizeBytes);

        try
        {
            await using var source = await _httpClient.GetStreamAsync(profile.DownloadUrl, cancellationToken);
            await using (var target = File.Create(temporaryPath))
            {
                await source.CopyToAsync(target, cancellationToken);
            }

            var actualSize = new FileInfo(temporaryPath).Length;
            var verified = VerifySha256(temporaryPath, profile.Sha256);
            if (verified)
            {
                File.Move(temporaryPath, path, overwrite: true);
            }
            else
            {
                TryDelete(temporaryPath);
            }

            stopwatch.Stop();
            _logger.LogInformation(
                "AI model download completed. ModelId={ModelId} Verified={Verified} ActualSizeBytes={ActualSizeBytes} DurationMs={DurationMs}",
                profile.ModelId,
                verified,
                actualSize,
                stopwatch.ElapsedMilliseconds);

            return new LocalAiModelDownloadResult
            {
                ModelId = profile.ModelId,
                Success = verified,
                LocalPath = path,
                Message = verified ? "Model pobrany i zweryfikowany." : "Pobrany model nie przeszedł weryfikacji SHA256."
            };
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            TryDelete(temporaryPath);
            _logger.LogWarning(
                "AI model download cancelled. ModelId={ModelId} DurationMs={DurationMs}",
                profile.ModelId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            TryDelete(temporaryPath);
            _logger.LogError(
                ex,
                "AI model download failed. ModelId={ModelId} DurationMs={DurationMs}",
                profile.ModelId,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public string GetLocalPath(LocalAiModelProfile profile) => Path.Combine(_modelDirectory, profile.FileName);

    private static bool IsConfigured(LocalAiModelProfile profile) =>
        !string.IsNullOrWhiteSpace(profile.DownloadUrl)
        && Uri.TryCreate(profile.DownloadUrl, UriKind.Absolute, out _)
        && !string.IsNullOrWhiteSpace(profile.Sha256);

    private static bool VerifySha256(string path, string expectedSha256)
    {
        if (string.IsNullOrWhiteSpace(expectedSha256))
        {
            return false;
        }

        using var stream = File.OpenRead(path);
        var hash = SHA256.HashData(stream);
        var actual = Convert.ToHexString(hash).ToLowerInvariant();
        return string.Equals(actual, expectedSha256.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase);
    }

    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch
        {
            // Cleanup failure is reported by the surrounding operation if it blocks progress.
        }
    }
}

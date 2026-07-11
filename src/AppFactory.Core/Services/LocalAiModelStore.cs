using System.Security.Cryptography;
using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class LocalAiModelStore
{
    private readonly HttpClient _httpClient;
    private readonly string _modelDirectory;

    public LocalAiModelStore(HttpClient? httpClient = null, string? modelDirectory = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _modelDirectory = modelDirectory ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "appfactory-models");
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
        await using var source = await _httpClient.GetStreamAsync(profile.DownloadUrl, cancellationToken);
        await using (var target = File.Create(path))
        {
            await source.CopyToAsync(target, cancellationToken);
        }

        var verified = VerifySha256(path, profile.Sha256);
        if (!verified)
        {
            File.Delete(path);
        }

        return new LocalAiModelDownloadResult
        {
            ModelId = profile.ModelId,
            Success = verified,
            LocalPath = path,
            Message = verified ? "Model pobrany i zweryfikowany." : "Pobrany model nie przeszedł weryfikacji SHA256."
        };
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
}

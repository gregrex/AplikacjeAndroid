namespace AppFactory.Mobile.Models;

public sealed class LocalAiModelProfile
{
    public string ModelId { get; init; } = string.Empty;
    public string Modality { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string DownloadUrl { get; init; } = string.Empty;
    public string Sha256 { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public bool IsRequiredForProduction { get; init; }
}

public sealed class LocalAiModelStatus
{
    public string ModelId { get; init; } = string.Empty;
    public bool IsConfigured { get; init; }
    public bool IsDownloaded { get; init; }
    public bool IsVerified { get; init; }
    public string LocalPath { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

public sealed class LocalAiModelDownloadResult
{
    public string ModelId { get; init; } = string.Empty;
    public bool Success { get; init; }
    public string LocalPath { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}

using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile.Services;

public sealed class LocalMediaInputService
{
    public Task<LocalMediaFile?> PickImageAsync(CancellationToken cancellationToken = default) =>
        PickAndCacheAsync(new PickOptions
        {
            PickerTitle = "Wybierz zdjęcie do analizy",
            FileTypes = FilePickerFileType.Images
        }, cancellationToken);

    public Task<LocalMediaFile?> PickAudioAsync(CancellationToken cancellationToken = default) =>
        PickAndCacheAsync(new PickOptions
        {
            PickerTitle = "Wybierz nagranie do analizy",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                [DevicePlatform.Android] = new[] { "audio/*" },
                [DevicePlatform.iOS] = new[] { "public.audio" },
                [DevicePlatform.WinUI] = new[] { ".wav", ".mp3", ".m4a", ".aac", ".ogg" },
                [DevicePlatform.MacCatalyst] = new[] { "public.audio" }
            })
        }, cancellationToken);

    private static async Task<LocalMediaFile?> PickAndCacheAsync(PickOptions options, CancellationToken cancellationToken)
    {
        var selected = await FilePicker.Default.PickAsync(options);
        if (selected is null)
        {
            return null;
        }

        var extension = Path.GetExtension(selected.FileName);
        var cachedName = $"ai-{Guid.NewGuid():N}{extension}";
        var cachedPath = Path.Combine(FileSystem.CacheDirectory, cachedName);

        await using var source = await selected.OpenReadAsync();
        await using var target = File.Create(cachedPath);
        await source.CopyToAsync(target, cancellationToken);

        var info = new FileInfo(cachedPath);
        return new LocalMediaFile
        {
            FileName = selected.FileName,
            LocalFilePath = cachedPath,
            ContentType = NormalizeContentType(selected.ContentType, extension),
            SizeBytes = info.Length
        };
    }

    private static string NormalizeContentType(string? contentType, string extension)
    {
        if (!string.IsNullOrWhiteSpace(contentType))
        {
            return contentType;
        }

        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".wav" => "audio/wav",
            ".mp3" => "audio/mpeg",
            ".m4a" or ".mp4" => "audio/mp4",
            ".aac" => "audio/aac",
            ".ogg" => "audio/ogg",
            _ => "application/octet-stream"
        };
    }
}

public sealed class LocalMediaFile
{
    public string FileName { get; init; } = string.Empty;
    public string LocalFilePath { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
}

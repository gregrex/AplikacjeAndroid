using System.Net;
using System.Security.Cryptography;
using AppFactory.Mobile.Models;
using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class LocalAiModelStoreTests
{
    [Fact]
    public void GetStatus_ReturnsNotConfigured_WhenModelHasNoUrlOrSha()
    {
        var store = new LocalAiModelStore(modelDirectory: Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        var profile = new LocalAiModelProfile
        {
            ModelId = "local-vision-v1",
            FileName = "local-vision-v1.onnx"
        };

        var status = store.GetStatus(profile);

        Assert.False(status.IsConfigured);
        Assert.False(status.IsDownloaded);
        Assert.False(status.IsVerified);
    }

    [Fact]
    public async Task DownloadAsync_ReturnsFailure_WhenModelHasNoUrlOrSha()
    {
        var store = new LocalAiModelStore(modelDirectory: Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")));
        var profile = new LocalAiModelProfile
        {
            ModelId = "local-audio-v1",
            FileName = "local-audio-v1.onnx"
        };

        var result = await store.DownloadAsync(profile);

        Assert.False(result.Success);
        Assert.Contains("URL", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DownloadAsync_SavesAndVerifiesConfiguredModel()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var handler = new StaticBytesHandler(bytes);
        var client = new HttpClient(handler);
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var store = new LocalAiModelStore(client, directory);
        var profile = new LocalAiModelProfile
        {
            ModelId = "local-test-v1",
            FileName = "local-test-v1.onnx",
            DownloadUrl = "https://models.example/local-test-v1.onnx",
            Sha256 = sha,
            SizeBytes = bytes.Length
        };

        var result = await store.DownloadAsync(profile);
        var status = store.GetStatus(profile);

        Assert.True(result.Success);
        Assert.True(File.Exists(result.LocalPath));
        Assert.True(status.IsDownloaded);
        Assert.True(status.IsVerified);
    }

    private sealed class StaticBytesHandler : HttpMessageHandler
    {
        private readonly byte[] _bytes;

        public StaticBytesHandler(byte[] bytes)
        {
            _bytes = bytes;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(_bytes)
            });
        }
    }
}

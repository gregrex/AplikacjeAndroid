using AppFactory.Mobile.Services;

namespace AppFactory.Mobile.Tests;

public sealed class LocalAiInputTensorFactoryTests
{
    [Fact]
    public void CreateNormalizedByteTensor_ReturnsExpectedLengthAndRange()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".bin");
        File.WriteAllBytes(path, new byte[] { 0, 127, 255 });
        var factory = new LocalAiInputTensorFactory();

        var tensor = factory.CreateNormalizedByteTensor(path, length: 6);

        Assert.Equal(6, tensor.Length);
        Assert.Equal(0f, tensor[0]);
        Assert.InRange(tensor[1], 0.49f, 0.50f);
        Assert.Equal(1f, tensor[2]);
        Assert.All(tensor, value => Assert.InRange(value, 0f, 1f));
    }

    [Fact]
    public void CreateNormalizedByteTensor_ThrowsForMissingFile()
    {
        var factory = new LocalAiInputTensorFactory();

        Assert.Throws<FileNotFoundException>(() => factory.CreateNormalizedByteTensor("missing-file.bin"));
    }
}

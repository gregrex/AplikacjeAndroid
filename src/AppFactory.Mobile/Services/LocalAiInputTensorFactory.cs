namespace AppFactory.Mobile.Services;

public sealed class LocalAiInputTensorFactory
{
    public const string DefaultInputName = "input";
    public static readonly int[] DefaultInputShape = { 1, 1, 1, 256 };

    public float[] CreateNormalizedByteTensor(string localFilePath, int length = 256)
    {
        if (string.IsNullOrWhiteSpace(localFilePath) || !File.Exists(localFilePath))
        {
            throw new FileNotFoundException("Input file not found.", localFilePath);
        }

        var bytes = File.ReadAllBytes(localFilePath);
        var data = new float[length];
        if (bytes.Length == 0)
        {
            return data;
        }

        for (var i = 0; i < data.Length; i++)
        {
            data[i] = bytes[i % bytes.Length] / 255f;
        }

        return data;
    }
}

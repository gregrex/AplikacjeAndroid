using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AppFactory.Mobile.Services;

public sealed class OnnxModelRunner
{
    public IReadOnlyList<float> RunSingleFloatTensor(string modelPath, string inputName, int[] inputShape, float[] inputData)
    {
        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException("ONNX model file not found.", modelPath);
        }

        using var session = new InferenceSession(modelPath);
        var tensor = new DenseTensor<float>(inputData, inputShape);
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(inputName, tensor)
        };

        using var results = session.Run(inputs);
        var first = results.FirstOrDefault();
        if (first is null)
        {
            return Array.Empty<float>();
        }

        return first.AsEnumerable<float>().ToArray();
    }
}

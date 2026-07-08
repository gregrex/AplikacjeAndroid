using AppFactory.Mobile.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace AppFactory.Mobile.Services;

public sealed class ClipboardExportService
{
    public async Task CopyResultAsync(ResultDefinition result)
    {
        var text = BuildResultText(result);
        if (!string.IsNullOrWhiteSpace(text))
        {
            await Clipboard.Default.SetTextAsync(text);
        }
    }

    private static string BuildResultText(ResultDefinition result)
    {
        var lines = new List<string>
        {
            result.Title,
            string.Empty,
            result.Summary,
            string.Empty
        };

        lines.AddRange(result.Steps.Select(x => $"- {x}"));

        return string.Join(Environment.NewLine, lines);
    }
}

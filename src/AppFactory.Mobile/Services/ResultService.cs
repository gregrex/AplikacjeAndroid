using AppFactory.Mobile.Models;

namespace AppFactory.Mobile.Services;

public sealed class ResultService
{
    public ResultDefinition? FindResult(IEnumerable<ResultDefinition> results, string resultId)
    {
        return results.FirstOrDefault(x => string.Equals(x.Id, resultId, StringComparison.OrdinalIgnoreCase));
    }
}

namespace AppFactory.Mobile.Services;

public sealed class MockAdService
{
    public Task<bool> ShowRewardedAsync(string placement)
    {
        Console.WriteLine($"[MockAd] Rewarded ad completed. Placement: {placement}");
        return Task.FromResult(true);
    }
}

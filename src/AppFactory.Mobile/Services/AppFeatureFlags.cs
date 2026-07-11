namespace AppFactory.Mobile.Services;

public sealed class AppFeatureFlags
{
#if DEBUG || LOCAL_AI_RELEASE
    public bool LocalAiEnabled => true;
#else
    public bool LocalAiEnabled => false;
#endif

#if DEBUG
    public bool DeveloperDiagnosticsEnabled => true;
#else
    public bool DeveloperDiagnosticsEnabled => false;
#endif

    public bool AdsEnabled => false;
    public bool FullResultsEnabled => true;
}

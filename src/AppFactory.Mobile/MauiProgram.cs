using AppFactory.Mobile.Services;
using Microsoft.Extensions.Logging;

namespace AppFactory.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ProjectDataService>();
        builder.Services.AddSingleton<RuleEngineService>();
        builder.Services.AddSingleton<ResultService>();
        builder.Services.AddSingleton<MockAdService>();
        builder.Services.AddSingleton<LocalDatabaseService>();
        builder.Services.AddSingleton<HistoryService>();
        builder.Services.AddSingleton<FavoritesService>();
        builder.Services.AddSingleton<LanguageService>();
        builder.Services.AddSingleton<ProjectThemeService>();
        builder.Services.AddSingleton<ProjectCatalogService>();
        builder.Services.AddSingleton<ProjectContextService>();
        builder.Services.AddSingleton<ProjectUiProfileService>();
        builder.Services.AddSingleton<ClipboardExportService>();
        builder.Services.AddSingleton<ResultNavigationStateService>();
        builder.Services.AddSingleton<BuildProfileService>();
        builder.Services.AddSingleton<ProjectToolStateService>();
        builder.Services.AddSingleton<LocalMediaInputService>();
        builder.Services.AddSingleton<AiSuggestionStateService>();
        builder.Services.AddSingleton<LocalAiModelCatalogService>();
        builder.Services.AddSingleton<LocalAiModelStore>();
        builder.Services.AddSingleton<OnnxModelRunner>();
        builder.Services.AddSingleton<LocalAiInputTensorFactory>();
        builder.Services.AddSingleton<ImageAnalysisPolicyService>();
        builder.Services.AddSingleton<ILocalVisionInferenceEngine, LocalVisionInferenceEngine>();
        builder.Services.AddSingleton<IImageAnalysisProvider, OnDeviceImageAnalysisProvider>();
        builder.Services.AddSingleton<ImageAnalysisService>();
        builder.Services.AddSingleton<AudioAnalysisPolicyService>();
        builder.Services.AddSingleton<ILocalAudioInferenceEngine, LocalAudioInferenceEngine>();
        builder.Services.AddSingleton<IAudioAnalysisProvider, OnDeviceAudioAnalysisProvider>();
        builder.Services.AddSingleton<AudioAnalysisService>();

        return builder.Build();
    }
}

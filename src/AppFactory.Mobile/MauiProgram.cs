using AppFactory.Mobile.Services;
using AppFactory.Persistence.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace AppFactory.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        const LogLevel localMinimumLevel = LogLevel.Debug;
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#else
        const LogLevel localMinimumLevel = LogLevel.Information;
#endif

        var localLogStore = new LocalLogStore(
            Path.Combine(FileSystem.AppDataDirectory, "logs"),
            new LocalLogOptions
            {
                MinimumLevel = localMinimumLevel,
                RetentionDays = 7,
                MaxFiles = 12,
                MaxFileSizeBytes = 2 * 1024 * 1024
            });

        builder.Services.AddSingleton(localLogStore);
        builder.Services.AddSingleton<DiagnosticsExportService>();
        builder.Logging.AddProvider(new LocalFileLoggerProvider(localLogStore));
        builder.Logging.SetMinimumLevel(localMinimumLevel);

        builder.Services.AddSingleton<AppFeatureFlags>();
        builder.Services.AddSingleton<ProjectDataService>();
        builder.Services.AddSingleton<RuleEngineService>();
        builder.Services.AddSingleton<ResultService>();
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
        builder.Services.AddSingleton(sp => new LocalAiModelStore(
            logger: sp.GetRequiredService<ILogger<LocalAiModelStore>>()));
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

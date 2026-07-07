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
        builder.Services.AddSingleton<HistoryService>();
        builder.Services.AddSingleton<FavoritesService>();

        return builder.Build();
    }
}

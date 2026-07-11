using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;

namespace AppFactory.Mobile;

public partial class App : Application
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
        InitializeComponent();
        RegisterGlobalExceptionHandlers();
        MainPage = new MainPage();

        _logger.LogInformation(
            "Application started. Version={Version} Build={Build} Package={Package}",
            AppInfo.Current.VersionString,
            AppInfo.Current.BuildString,
            AppInfo.Current.PackageName);
    }

    private void RegisterGlobalExceptionHandlers()
    {
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

#if ANDROID
        Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += OnAndroidUnhandledException;
#endif
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        if (args.ExceptionObject is Exception exception)
        {
            _logger.LogCritical(exception, "Unhandled application-domain exception. IsTerminating={IsTerminating}", args.IsTerminating);
        }
        else
        {
            _logger.LogCritical("Unhandled non-Exception object. IsTerminating={IsTerminating} Value={Value}", args.IsTerminating, args.ExceptionObject);
        }
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        _logger.LogError(args.Exception, "Unobserved task exception.");
        args.SetObserved();
    }

#if ANDROID
    private void OnAndroidUnhandledException(object? sender, Android.Runtime.RaiseThrowableEventArgs args)
    {
        _logger.LogCritical(args.Exception, "Unhandled Android runtime exception.");
    }
#endif
}

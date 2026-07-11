using Microsoft.Extensions.Logging;

namespace AppFactory.Persistence.Diagnostics;

public sealed class LocalFileLoggerProvider : ILoggerProvider
{
    private readonly LocalLogStore _store;

    public LocalFileLoggerProvider(LocalLogStore store)
    {
        _store = store;
    }

    public ILogger CreateLogger(string categoryName) => new LocalFileLogger(categoryName, _store);

    public void Dispose()
    {
    }

    private sealed class LocalFileLogger : ILogger
    {
        private readonly string _category;
        private readonly LocalLogStore _store;

        public LocalFileLogger(string category, LocalLogStore store)
        {
            _category = category;
            _store = store;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => _store.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            _store.Write(logLevel, _category, eventId, message, exception);
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}

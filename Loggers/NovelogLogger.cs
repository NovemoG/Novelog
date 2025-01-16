using Microsoft.Extensions.Logging;

namespace Novelog;

/// <summary>
/// A logger for Novelog using Microsoft.Extensions.Logging.
/// You may want to use separate configBuilder for this, since atLine does not work.
/// </summary>
/// <param name="baseLogger">The base logger to use.</param>
/// <param name="categoryName">The category name of the logger.</param>
public class NovelogLogger(Logger baseLogger, string categoryName) : ILogger
{
    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) => new LoggerScope<TState>(state);

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        //if (!IsEnabled(logLevel)) return;
        
        var message = formatter(state, exception);

        var scope = LoggerScope<TState>.Current;
        if (scope != null)
        {
            message = $"<{scope.State}> {message}";
        }
        
        switch (logLevel)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
                baseLogger.LogDebug(message, caller: categoryName);
                break;
            case LogLevel.Warning:
                baseLogger.LogWarning(message, exception, caller: categoryName);
                break;
            case LogLevel.Error:
                baseLogger.LogError(message, exception, caller: categoryName);
                break;
            case LogLevel.Critical:
                baseLogger.LogCritical(message, exception, caller: categoryName);
                break;
            case LogLevel.Information:
            case LogLevel.None:
            default:
                baseLogger.LogInfo(message, caller: categoryName);
                break;
        }
    }

    internal class LoggerScope<TState> : IDisposable
    {
        private static readonly AsyncLocal<LoggerScope<TState>?> CurrentScope = new();
        private readonly LoggerScope<TState>? _parentScope;
        
        private bool _disposed;

        public TState State { get; }

        public LoggerScope(TState state)
        {
            State = state;
            _parentScope = CurrentScope.Value;
            CurrentScope.Value = this;
        }

        public static LoggerScope<TState>? Current => CurrentScope.Value;

        public void Dispose()
        {
            if (_disposed) return;
            CurrentScope.Value = _parentScope;
            _disposed = true;
        }
    }
}
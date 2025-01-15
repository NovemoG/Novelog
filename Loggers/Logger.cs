using System.Runtime.CompilerServices;
using Novelog.Abstractions;
using Novelog.Config;
using Novelog.Formatters;
using Novelog.Types;

namespace Novelog;

/// <summary>
/// A simple logger that can log to multiple handlers. It is recommended to use the DI implementation for this logger.
/// </summary>
public sealed class Logger : ILogger
{
    #region Shared

    private static ILogger? _shared;

    /// <summary>
    /// Contains a shared logger instance that can be used throughout the application.
    /// This will be the same instance as the one created by the first instantiation
    /// of the <see cref="LoggerConfigBuilder"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the shared logger is accessed before it is set.</exception>
    public static ILogger Shared
    {
        get => _shared ?? throw new InvalidOperationException("Tried to access shared logger before it was set.");
        internal set
        {
            if (_shared is not null)
            {
                _shared.LogWarning("Tried to set shared logger when it was already set.");
            }
            else
            {
                _shared = value;
            }
        }
    }
    
    #endregion
    
    internal Logger() { }
    
    internal List<LogHandler> LogHandlers { get; } = [];
    internal IFormatter Formatter { get; set; } = new DefaultFormatter();
    
    private bool _disposed;

    #region Log Handlers
    
    private void Log(
        LogLevel level,
        string message,
        string caller,
        int atLine)
    {
        Formatter.Format(ref message, level, caller, atLine);

        foreach (var handler in LogHandlers)
        {
            if (level < handler.MinLogLevel) continue;
            
            handler.Log(level, message);
        }
    }

    private void LogWithException(
        LogLevel level,
        string message,
        Exception? ex,
        string caller,
        int atLine)
    {
        if (ex is not null)
        {
            var fullMessage = string.IsNullOrWhiteSpace(ex.StackTrace)
                ? $"{message}\n{ex.Message}"
                : $"{message}\n{ex.Message}\n{ex.StackTrace}";
            
            Log(level, fullMessage, caller, atLine);
        }
        else
        {
            Log(level, message, caller, atLine);
        }
    }

    #endregion
    
    #region No parameters implementation

    /// <inheritdoc />
    public void LogDebug(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        Log(LogLevel.DEBUG, message, caller, atLine);
    }


    /// <inheritdoc />
    public void LogInfo(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        Log(LogLevel.INFO, message, caller, atLine);
    }


    /// <inheritdoc />
    public void LogWarning(string message, Exception? ex = null,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.WARN, message, ex, caller, atLine);
    }


    /// <inheritdoc />
    public void LogError(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.ERROR, message, ex, caller, atLine);
    }


    /// <inheritdoc />
    public void LogCritical(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.CRIT, message, ex, caller, atLine);
    }

    #endregion

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        
        foreach (var handler in LogHandlers)
        {
            if (handler is IDisposable disposableHandler)
            {
                disposableHandler.Dispose();
            }
        }

        _disposed = true;
    }
}
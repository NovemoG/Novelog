using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
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
    
    internal Logger() { }
    
    #endregion
    
    internal List<LogHandler> LogHandlers { get; } = [];
    internal IFormatter Formatter { get; set; } = new DefaultFormatter();

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
    
    /// <summary>
    /// Logs a debug message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogDebug(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        Log(LogLevel.DEBUG, message, caller, atLine);
    }
    
    /// <summary>
    /// Logs an info message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogInfo(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        Log(LogLevel.INFO, message, caller, atLine);
    }
    
    /// <summary>
    /// Logs a warning message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogWarning(string message, Exception? ex = null,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.WARN, message, ex, caller, atLine);
    }
    
    /// <summary>
    /// Logs an error message with an exception and stacktrace to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogError(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.ERROR, message, ex, caller, atLine);
    }
    
    /// <summary>
    /// Logs a critical error message to all attached log handlers.
    /// (Should be used for fatal errors that stop the application)
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogCritical(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        LogWithException(LogLevel.CRIT, message, ex, caller, atLine);
    }

    #endregion
}

#region DI Implementation

// ReSharper disable once InconsistentNaming
/// <summary>
/// Contains extension method for the <see cref="IServiceCollection"/> to add the logger to the DI.
/// </summary>
public static class LoggerDI
{
    /// <summary>
    /// Adds the logger to the DI with the specified configuration.
    /// </summary>
    /// <param name="services">The service collection to add the logger to.</param>
    /// <param name="options">The configuration options for the logger.</param>
    public static void AddLogger(this IServiceCollection services, Action<LoggerConfigBuilder> options)
    {
        var configBuilder = new LoggerConfigBuilder();
        options(configBuilder);
        
        services.AddSingleton<ILogger>(configBuilder.Build());
    }
}

#endregion
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
public class Logger : ILogger
{
    internal List<LogHandler> LogHandlers { get; } = [];
    
    internal IFormatter Formatter { get; set; } = new DefaultFormatter();

    private void Log(LogLevel level, string message, string caller, int atLine)
    {
        Formatter.Format(ref message, level, caller, atLine);

        foreach (var handler in LogHandlers)
        {
            if (level < handler.MinLogLevel) continue;
            
            handler.Log(level, message);
        }
    }

    #region No parameters methods 
    
    /// <summary>
    /// Logs a debug message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogDebug(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
        => Log(LogLevel.DEBUG, message, caller, atLine);
    
    /// <summary>
    /// Logs an info message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogInfo(string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
        => Log(LogLevel.INFO, message, caller, atLine);
    
    /// <summary>
    /// Logs a warning message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogWarning(string message, Exception? ex = null,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        if (ex is not null)
        {
            var fullMessage = $"{message}\n{ex.Message}\n{ex.StackTrace}";
            Log(LogLevel.WARN, fullMessage, caller, atLine);
        }
        else
        {
            Log(LogLevel.WARN, message, caller, atLine);
        }
    }
    
    /// <summary>
    /// Logs an error message with an exception and stacktrace to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogError(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        if (ex is not null)
        {
            var fullMessage = $"{message}\n{ex.Message}\n{ex.StackTrace}";
            Log(LogLevel.ERROR, fullMessage, caller, atLine);
        }
        else
        {
            Log(LogLevel.ERROR, message, caller, atLine);
        }
    }
    
    /// <summary>
    /// Logs a critical error message to all attached log handlers.
    /// (Should be used for fatal errors that stop the application)
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogCritical(string message, Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        if (ex is not null)
        {
            var fullMessage = $"{message}\n{ex.Message}\n{ex.StackTrace}";
            Log(LogLevel.CRIT, fullMessage, caller, atLine);
        }
        else
        {
            Log(LogLevel.CRIT, message, caller, atLine);
        }
    }

    #endregion
}

#region DI Implementation

// ReSharper disable once InconsistentNaming
public static class LoggerDI
{
    public static void AddLogger(this IServiceCollection services, Action<LoggerConfigBuilder> options)
    {
        var configBuilder = new LoggerConfigBuilder();
        options(configBuilder);
        
        services.AddSingleton<ILogger>(configBuilder.Build());
    }
}

#endregion
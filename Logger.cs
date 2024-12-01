using Microsoft.Extensions.DependencyInjection;
using Novelog.Abstractions;
using Novelog.Config;
using Novelog.Types;

namespace Novelog;

/// <summary>
/// A simple logger that can log to multiple handlers. It is recommended to use the DI implementation for this logger.
/// </summary>
public class Logger : ILogger
{
    internal List<LogHandler> LogHandlers { get; } = [];
    
    internal string DateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
    internal string LogMessageFormat { get; set; } = "[{0}] [{3}] {4}";

    private void Log(LogLevel level, string message, string caller, int atLine)
    {
        var timestamp = DateTime.Now.ToString(DateFormat);
        var logMessage = string.Format(LogMessageFormat, timestamp, caller, atLine, level, message);

        foreach (var handler in LogHandlers)
        {
            if (level < handler.MinLogLevel) continue;
            
            handler.Log(level, logMessage);
        }
    }
    
    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogDebug(string message, string caller = "", int atLine = 0) => Log(LogLevel.DEBUG, message, caller, atLine);
    
    /// <summary>
    /// Logs an info message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogInfo(string message, string caller = "", int atLine = 0) => Log(LogLevel.INFO, message, caller, atLine);
    
    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogWarning(string message, string caller = "", int atLine = 0) => Log(LogLevel.WARN, message, caller, atLine);
    
    /// <summary>
    /// Logs an error message with an exception and stacktrace.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogError(string message, Exception ex, string caller = "", int atLine = 0)
    {
        var fullMessage = $"{message}\n{ex.Message}\n{ex.StackTrace}";
        Log(LogLevel.ERROR, fullMessage, caller, atLine);
    }
    
    /// <summary>
    /// Logs a critical error message. (Should be used for fatal errors that stop the application)
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    public void LogCritical(string message, string caller = "", int atLine = 0) => Log(LogLevel.CRIT, message, caller, atLine);
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
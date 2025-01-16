using System.Runtime.CompilerServices;

namespace Novelog.Abstractions;

/// <summary>
/// Logger's abstraction containing all the methods' definitions used for logging.
/// </summary>
public interface ILogger : IDisposable
{
    /// <summary>
    /// Logs a debug message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogDebug(
        string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs an info message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogInfo(
        string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);

    /// <summary>
    /// Logs a warning message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogWarning(
        string message,
        Exception? ex = null,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs an error message with an exception and stacktrace to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogError(
        string message,
        Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs a critical error message to all attached log handlers.
    /// (Should be used for fatal errors that stop the application)
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="caller">The method or any other identifier that called the log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogCritical(
        string message,
        Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
}

/// <summary>
/// Logger's abstraction containing all the methods' definitions used for logging.
/// </summary>
public interface ILogger<T> : IDisposable
{
    /// <summary>
    /// Logs a debug message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogDebug(
        string message,
        string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs an info message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogInfo(
        string message,
        string caller = "",
        [CallerLineNumber] int atLine = 0);

    /// <summary>
    /// Logs a warning message to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogWarning(
        string message,
        Exception? ex = null,
        string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs an error message with an exception and stacktrace to all attached log handlers.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogError(
        string message,
        Exception? ex,
        string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    /// <summary>
    /// Logs a critical error message to all attached log handlers.
    /// (Should be used for fatal errors that stop the application)
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">The exception to log.</param>
    /// <param name="atLine">The line at which the log was called.</param>
    void LogCritical(
        string message,
        Exception? ex,
        string caller = "",
        [CallerLineNumber] int atLine = 0);
}
using Novelog.Types;

namespace Novelog.Abstractions;

/// <summary>
/// An abstract handler for logging messages. Implement this class to create custom log handlers.
/// </summary>
public abstract class LogHandler
{
    /// <summary>
    /// Minimum log level needed to call the log handler.
    /// </summary>
    public virtual LogLevel MinLogLevel => LogLevel.DEBUG;
    
    /// <summary>
    /// Log message handler.
    /// </summary>
    /// <param name="level">Log level.</param>
    /// <param name="message">Formatted log message.</param>
    public abstract void Log(LogLevel level, string message);
}
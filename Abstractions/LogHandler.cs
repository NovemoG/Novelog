using Novelog.Types;

namespace Novelog.Abstractions;

/// <summary>
/// A handler for logging messages. Implement this class to create custom log handlers.
/// </summary>
public abstract class LogHandler
{
    public virtual LogLevel MinLogLevel => LogLevel.DEBUG;
    
    public abstract void Log(LogLevel level, string message);
}
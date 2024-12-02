using Novelog.Types;

namespace Novelog.Abstractions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Formatter abstraction for the message content of the logs.
/// </summary>
public interface IFormatter
{
    void Format(ref string message, LogLevel level, string caller = "", int atLine = 0);
    
    void Format<T0>(ref string message, LogLevel level, T0 arg, string caller = "", int atLine = 0);
    
    void Format<T0, T1>(ref string message, LogLevel level, T0 arg0, T1 arg1, string caller = "", int atLine = 0);

    void Format(ref string message, LogLevel level, string caller = "", int atLine = 0, params object?[]? args);
}
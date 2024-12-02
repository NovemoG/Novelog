using System.Runtime.CompilerServices;

namespace Novelog.Abstractions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Logger's abstraction containing all the methods definitions used for logging.
/// </summary>
public interface ILogger
{
    void LogDebug(
        string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    void LogInfo(
        string message,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);

    void LogWarning(
        string message,
        Exception? ex = null,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    void LogError(
        string message,
        Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
    
    void LogCritical(
        string message,
        Exception? ex,
        [CallerMemberName] string caller = "",
        [CallerLineNumber] int atLine = 0);
}
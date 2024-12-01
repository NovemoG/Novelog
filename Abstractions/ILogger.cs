using System.Runtime.CompilerServices;

namespace Novelog.Abstractions;

public interface ILogger
{
    void LogDebug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int atLine = 0);
    void LogInfo(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int atLine = 0);
    void LogWarning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int atLine = 0);
    void LogError(string message, Exception ex, [CallerMemberName] string caller = "", [CallerLineNumber] int atLine = 0);
    void LogCritical(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int atLine = 0);
}
using Novelog.Abstractions;
using Novelog.Types;

namespace Novelog.Formatters;

internal sealed class DefaultFormatter : IFormatter
{
    public string MessageFormat { get; init; } = "[{0}] [{1}:{2} | {3}] {4}";
    public string DateFormat { get; init; } = "yyyy-MM-dd HH:mm:ss";
    
    private string Timestamp => DateTime.Now.ToString(DateFormat);

    public void Format(
        ref string message,
        LogLevel level,
        string caller = "",
        int atLine = 0)
    {
        message = string.Format(MessageFormat, Timestamp, caller, atLine, level, message);
    }

    public void Format<T0>(
        ref string message,
        LogLevel level,
        T0 arg,
        string caller = "",
        int atLine = 0)
    {
        message += " With arg: " + arg;
        Format(ref message, level, caller, atLine);
    }

    public void Format<T0, T1>(
        ref string message,
        LogLevel level,
        T0 arg0,
        T1 arg1,
        string caller = "",
        int atLine = 0)
    {
        message += $" With args: {arg0}, {arg1}";
        Format(ref message, level, caller, atLine);
    }

    public void Format(
        ref string message,
        LogLevel level,
        string caller = "",
        int atLine = 0,
        params object?[]? args)
    {
        if (args is not null) message += $" With args: {string.Join(", ", args)}";
        Format(ref message, level, caller, atLine);
    }
}
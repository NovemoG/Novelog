using Novelog.Abstractions;

namespace Novelog.Types.Handlers;

internal class ConsoleLogHandler : LogHandler
{
    public override LogLevel MinLogLevel { get; }
    
    public ConsoleLogHandler(LogLevel minLogLevel)
    {
        MinLogLevel = minLogLevel;
    }
    
    public override void Log(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.DEBUG:
            case LogLevel.INFO:
                Console.WriteLine(message);
                break;
            case LogLevel.WARN:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogLevel.ERROR:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogLevel.CRIT:
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.BackgroundColor = ConsoleColor.Black;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }
}
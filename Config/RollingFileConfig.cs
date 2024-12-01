using Novelog.Types;

namespace Novelog.Config;

public record RollingFileConfig
{
    public LogLevel MinLogLevel { get; init; } = LogLevel.INFO;
    public required string FilePath { get; init; }
    public long MaxFileSize { get; init; } = 10 * 1024 * 1024;
    public int MaxFileCount { get; init; } = 7;
}
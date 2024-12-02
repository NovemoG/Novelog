using Novelog.Types;

namespace Novelog.Config;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
/// <summary>
/// Configuration for the rolling file handler.
/// </summary>
public record RollingFileConfig
{
    public LogLevel MinLogLevel { get; init; } = LogLevel.INFO;
    public required string FilePath { get; init; }
    /// <summary>
    /// File size in bytes
    /// </summary>
    public long MaxFileSize { get; init; } = 10 * 1024 * 1024;
    public int MaxFileCount { get; init; } = 7;
    /// <summary>
    /// Interval in milliseconds
    /// </summary>
    public int BufferFlushInterval { get; init; } = 250;
}
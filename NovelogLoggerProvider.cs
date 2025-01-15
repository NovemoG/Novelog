using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Novelog;

/// <summary>
/// A logger provider for Novelog using Microsoft.Extensions.Logging.
/// </summary>
/// <param name="defaultLogger">The default logger to use.</param>
public sealed class NovelogLoggerProvider(Logger defaultLogger) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, NovelogLogger> _loggers = new();

    private bool _disposed;

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, new NovelogLogger(defaultLogger, categoryName));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        defaultLogger.Dispose();
        _disposed = true;
    }
}
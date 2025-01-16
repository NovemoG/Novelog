using System.Runtime.CompilerServices;
using Novelog.Abstractions;
// ReSharper disable ExplicitCallerInfoArgument

namespace Novelog;

/// <summary>
/// A simple logger that can log to multiple handlers.
/// It is a type-specific logger that can be configured differently than the default logger.
/// <i>It can be only used in DI.</i>
/// </summary>
public sealed class TypedLogger<T> : ILogger<T>
{
    private readonly Logger _baseLogger;

    private bool _disposed;

    /// <summary>
    /// Creates a new instance of the <see cref="TypedLogger{T}"/> class.
    /// </summary>
    /// <param name="baseLogger">Logger to use for logging.</param>
    [Obsolete("This constructor is only used by DI.", true)]
    public TypedLogger(Logger baseLogger)
    {
        _baseLogger = baseLogger;
    }

    /// <inheritdoc />
    public void LogDebug(string message,
        string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        _baseLogger.LogDebug(message, GetCaller(caller), atLine);
    }

    /// <inheritdoc />
    public void LogInfo(string message,
        string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        _baseLogger.LogInfo(message, GetCaller(caller), atLine);
    }

    /// <inheritdoc />
    public void LogWarning(string message,Exception? ex = null,
        string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        _baseLogger.LogWarning(message, ex, GetCaller(caller), atLine);
    }

    /// <inheritdoc />
    public void LogError(string message, Exception? ex,
        string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        _baseLogger.LogError(message, ex, GetCaller(caller), atLine);
    }

    /// <inheritdoc />
    public void LogCritical(string message, Exception? ex,
        string caller = "",
        [CallerLineNumber] int atLine = 0)
    {
        _baseLogger.LogCritical(message, ex, GetCaller(caller), atLine);
    }
    
    private static string GetCaller(string caller)
        => string.IsNullOrEmpty(caller) ? typeof(T).Name : caller;
    
    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;
        _baseLogger.Dispose();
        _disposed = true;
    }
}
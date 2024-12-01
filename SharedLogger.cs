using Novelog.Abstractions;
using Novelog.Config;

namespace Novelog;

/// <summary>
/// Contains a shared logger instance that can be used throughout the application.<br/>
/// By default, the logger is configured to only log to the console.<br/>
/// It is strongly advised to configure the shared logger before using it.
/// </summary>
public static class SharedLogger
{
    private static bool _isConfigured;
    private static Lazy<ILogger> _logger = new(() => new LoggerConfigBuilder()
        .AttachConsole()
        .Build());

    public static ILogger Logger => _logger.Value;

    /// <summary>
    /// Configures the shared logger instance.
    /// If you're using the DI implementation, this method is not needed because your configured instance will be used.
    /// </summary>
    /// <param name="logger">The logger instance to use as the shared logger.</param>
    public static void Configure(Logger logger)
    {
        if (_isConfigured) return;

        _logger = new Lazy<ILogger>(() => logger);
        _isConfigured = true;
    }
}
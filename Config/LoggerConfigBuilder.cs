using Novelog.Abstractions;
using Novelog.Types;
using Novelog.Types.Handlers;

namespace Novelog.Config;

/// <summary>
/// A builder class for configuring the <see cref="Logger"/>.
/// </summary>
public class LoggerConfigBuilder
{
    private readonly Logger _logger = new();
    private bool _setupShared;
    
    /// <summary>
    /// Also configures the <see cref="SharedLogger"/> instance.
    /// </summary>
    public LoggerConfigBuilder SetupShared()
    {
        _setupShared = true;
        return this;
    }
    
    /// <summary>
    /// Sets the date time format for the logger.
    /// </summary>
    /// <param name="format">The format of the date time.</param>
    public LoggerConfigBuilder SetDateTimeFormat(string format)
    {
        _logger.DateFormat = format;
        return this;
    }
    
    /// <summary>
    /// Sets the format of the log message.<br/>
    /// Here is the list of supported inputs:<br/>
    /// {0} - Timestamp<br/>
    /// {1} - Caller (Method name by default)<br/>
    /// {2} - At what line was the log called<br/>
    /// {3} - Log level<br/>
    /// {4} - Message<br/>
    /// </summary>
    /// <param name="format">The format of the log message.</param>
    public LoggerConfigBuilder SetLogMessageFormat(string format)
    {
        _logger.LogMessageFormat = format;
        return this;
    }
    
    /// <summary>
    /// Attaches a console log handler to the logger.
    /// </summary>
    /// <param name="minLevel">The minimum log level to log.</param>
    public LoggerConfigBuilder AttachConsole(LogLevel minLevel = LogLevel.DEBUG)
    {
        _logger.LogHandlers.Add(new ConsoleLogHandler(minLevel));
        return this;
    }
    
    /// <summary>
    /// Attaches a rolling file log handler to the logger.
    /// Default setup is as follows:
    /// - Max file size: 10MB
    /// - Max file count: 5
    /// - Min log level: INFO
    /// </summary>
    /// <param name="config">The configuration for the rolling file log handler.</param>
    public LoggerConfigBuilder AttachRollingFile(RollingFileConfig config)
    {
        _logger.LogHandlers.Add(new RollingFileLogHandler(config));
        return this;
    }
    
    /// <summary>
    /// Attaches a custom log handler to the logger.
    /// </summary>
    /// <param name="handler">The custom log handler.</param>
    public LoggerConfigBuilder AttachLogMethod(LogHandler handler)
    {
        _logger.LogHandlers.Add(handler);
        return this;
    }

    /// <summary>
    /// Finalizes the logger configuration and builds the logger.
    /// </summary>
    /// <returns>The built logger.</returns>
    public Logger Build()
    {
        if (_setupShared)
        {
            SharedLogger.Configure(_logger);
        }
        
        return _logger;
    }
}
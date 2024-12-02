﻿using Novelog.Abstractions;
using Novelog.Formatters;
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
    /// Sets the formatter of the logger.
    /// </summary>
    /// <param name="formatter">The formatter to use.</param>
    public LoggerConfigBuilder SetMessageFormatter(IFormatter formatter)
    {
        _logger.Formatter = formatter;
        return this;
    }
    
    /// <summary>
    /// Modifies the default formatter of the logger.
    /// You don't have to set both parameters just leave one empty if desired.
    /// <b>If you set custom formatter don't use this method!</b><br/>
    /// Here is the list of supported inputs for message format:<br/>
    /// {0} - Timestamp<br/>
    /// {1} - Caller (Method name by default)<br/>
    /// {2} - At what line was the log called<br/>
    /// {3} - Log level<br/>
    /// {4} - Message<br/>
    /// </summary>
    /// <param name="dateFormat">The date format to use. <i>(Default: [{0}] [{1}:{2} | {3}] {4})</i></param>
    /// <param name="messageFormat">The message format to use. <i>(Default: yyyy-MM-dd HH:mm:ss)</i></param>
    public LoggerConfigBuilder ModifyDefaultFormatter(string dateFormat, string messageFormat)
    {
        var isDateFormatEmpty = string.IsNullOrWhiteSpace(dateFormat);
        var isMessageFormatEmpty = string.IsNullOrWhiteSpace(messageFormat);

        _logger.Formatter = isDateFormatEmpty switch
        {
            false when !isMessageFormatEmpty => new DefaultFormatter
            {
                DateFormat = dateFormat, MessageFormat = messageFormat
            },
            true when !isMessageFormatEmpty => new DefaultFormatter { MessageFormat = messageFormat },
            false when isMessageFormatEmpty => new DefaultFormatter { DateFormat = dateFormat },
            _ => _logger.Formatter
        };

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
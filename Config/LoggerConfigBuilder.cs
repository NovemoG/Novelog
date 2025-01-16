using Novelog.Abstractions;
using Novelog.Formatters;
using Novelog.Types.Handlers;
using LogLevel = Novelog.Types.LogLevel;

namespace Novelog.Config;

/// <summary>
/// A builder class for configuring the <see cref="Logger"/>.
/// </summary>
public sealed class LoggerConfigBuilder
{
    private readonly Logger _logger = new();
    private readonly Dictionary<Type, LoggerConfigBuilder> _typedConfigs = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggerConfigBuilder"/> class.
    /// After this constructor is called, the shared logger will be set to the logger being built.
    /// </summary>
    public LoggerConfigBuilder(bool setShared = true)
    {
        if (!setShared) return;
        Logger.Shared = _logger;
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
    /// <b>If you set custom formatter don't use this method as it will override the custom formatter.</b><br/>
    /// Here is the list of supported inputs for message format:<br/>
    /// {0} - Timestamp<br/>
    /// {1} - Caller (Method name by default)<br/>
    /// {2} - At what line was the log called<br/>
    /// {3} - Log level<br/>
    /// {4} - Message
    /// </summary>
    /// <param name="dateFormat">The date format to use. <i>(Default: yyyy-MM-dd HH:mm:ss)</i></param>
    /// <param name="messageFormat">The message format to use. <i>(Default: [{0}] [{1}:{2} | {3}] {4})</i></param>
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
    /// Default setup is as follows:<br/>
    /// - Max file size: 10MB<br/>
    /// - Max file count: 5<br/>
    /// - Min log level: INFO<br/>
    /// - Buffer flush interval: 250ms
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
    /// Allows configuring a separate logger for type <typeparamref name="T"/>.
    /// </summary>
    public LoggerConfigBuilder ForType<T>(Action<LoggerConfigBuilder> options)
    {
        var typeBuilder = new LoggerConfigBuilder(false);
        options(typeBuilder);
        
        _typedConfigs[typeof(T)] = typeBuilder;

        return this;
    }

    /// <summary>
    /// Finalizes the logger configuration and builds the logger.
    /// </summary>
    /// <returns>The built logger.</returns>
    public Logger Build()
    {
        return _logger;
    }

    /// <summary>
    /// Builds the default logger and any typed logger configurations.
    /// </summary>
    /// <returns>
    /// A default logger and a dictionary mapping each
    /// configured type to its own Logger.
    /// </returns>
    public (Logger DefaultLogger, Dictionary<Type, Logger> Loggers) BuildAll()
    {
        var defaultLogger = _logger;
        var typedLoggers = new Dictionary<Type, Logger>();

        foreach (var (type, subBuilder) in _typedConfigs)
        {
            typedLoggers[type] = subBuilder._logger;
        }
        
        return (defaultLogger, typedLoggers);
    }
}
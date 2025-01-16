using Microsoft.Extensions.DependencyInjection;
using Novelog.Config;
using ILogger = Novelog.Abstractions.ILogger;

namespace Novelog;

/// <summary>
/// Contains extension methods for adding the logger to the DI.
/// </summary>
public static class LoggerDI
{
    /// <summary>
    /// Adds the logger to the DI with the specified configuration.
    /// </summary>
    /// <param name="services">The service collection to add the logger to.</param>
    /// <param name="options">The configuration options for the logger.</param>
    public static void AddNovelog(this IServiceCollection services, Action<LoggerConfigBuilder> options)
    {
        var configBuilder = new LoggerConfigBuilder();
        options(configBuilder);
        
        services.AddNovelog(configBuilder);
    }

    /// <summary>
    /// Adds the logger to the DI with the specified configuration.
    /// </summary>
    /// <param name="services">The service collection to add the logger to.</param>
    /// <param name="configBuilder">The configuration options for the logger.</param>
    /// <exception cref="InvalidOperationException">Logger for specific type couldn't be created</exception>
    public static void AddNovelog(this IServiceCollection services, LoggerConfigBuilder configBuilder)
    {
        var (defaultLogger, typedLoggers) = configBuilder.BuildAll();

        services.AddSingleton<ILogger>(defaultLogger);
        
        foreach (var (type, logger) in typedLoggers)
        {
            var typedLoggerInterface = typeof(Abstractions.ILogger<>).MakeGenericType(type);
            var typedLoggerClass = typeof(TypedLogger<>).MakeGenericType(type);
            
            var typedLoggerInstance = Activator.CreateInstance(typedLoggerClass, logger);
            if (typedLoggerInstance == null)
            {
                throw new InvalidOperationException($"Failed to create TypedLogger for {type.Name}.");
            }
            
            services.AddSingleton(typedLoggerInterface, typedLoggerInstance);
        }
    }
}
# Novelog

Novelog is a simple and flexible logging library for .NET applications.
It supports multiple log handlers, message enrichment, and filtering.

## Features

- Log to multiple handlers (e.g., console, file)
- Rolling file log handler with size-based rotation
- Custom log handlers

## Installation

To install Novelog, add the following NuGet package to your project:

```sh
dotnet add package Novelog
```

## Usage

### Basic Configuration

To use the logger, you need to configure it first. You can configure the logger using Dependency Injection (DI) or manually.

#### Using Dependency Injection

1. Add the logger to your service collection `Program.cs`:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Novelog;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogger(config =>
        {
            config.AttachConsole();
            config.SetupShared();
        });
    }
}
```

2. Inject and use the logger in your classes:

```csharp
using Novelog;

public class MyClass
{
    private readonly ILogger _logger;

    public MyClass(ILogger logger)
    {
        _logger = logger;
    }

    public void DoSomething()
    {
        _logger.LogInfo("This is an info message.");
    }
}
```

#### Manual Configuration

1. Configure the logger manually:

```csharp
using Novelog;
using Novelog.Config;

var logger = new LoggerConfigBuilder()
    .AttachConsole()
    .Build();

SharedLogger.Configure(logger);
```

2. Use the shared logger:

```csharp
using Novelog;

public class MyClass
{
    public void DoSomething()
    {
        SharedLogger.Logger.LogInfo("This is an info message.");
    }
}
```

### Rolling File Log Handler

The rolling file log handler rotates log files based on size. Configure it using the `RollingFileConfig` class:

```csharp
using Novelog.Config;

var rollingFileConfig = new RollingFileConfig
{
    FilePath = "logs/log.txt",
    MaxFileSize = 10 * 1024 * 1024,
    MaxFileCount = 5,
    MinLogLevel = LogLevel.INFO
};

var logger = new LoggerConfigBuilder()
    .AttachRollingFile(rollingFileConfig)
    .Build();

SharedLogger.Configure(logger);
```

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

---
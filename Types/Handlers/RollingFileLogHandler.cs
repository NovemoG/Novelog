using System.Collections.Concurrent;
using Novelog.Abstractions;
using Novelog.Config;
using Novelog.Extensions;

namespace Novelog.Types.Handlers;

internal class RollingFileLogHandler : LogHandler, IDisposable
{
    public override LogLevel MinLogLevel { get; }
    private readonly long _maxFileSize;
    private readonly int _maxFileCount;
    
    private readonly string _filename;
    private StreamWriter _writer;
    private int _fileIndex;
    
    private readonly ConcurrentQueue<string> _logQueue = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly Task _loggingTask;
    
    public RollingFileLogHandler(RollingFileConfig config)
    {
        MinLogLevel = config.MinLogLevel;
        _maxFileSize = config.MaxFileSize;
        _maxFileCount = config.MaxFileCount;
        _filename = config.FilePath;
        _writer = CreateStreamWriter();
        
        _loggingTask = Task.Run(ProcessLogQueue);
    }

    public override void Log(LogLevel level, string message)
    {
        _logQueue.Enqueue(message);
    }

    private async Task ProcessLogQueue()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            while (_logQueue.TryDequeue(out var logMessage))
            {
                await _writer.WriteLineAsync(logMessage);
                await _writer.FlushAsync();

                if (_writer.BaseStream.Length > _maxFileSize)
                {
                    await RollFile();
                }
            }
            await Task.Delay(1000);
        }
    }

    private StreamWriter CreateStreamWriter()
    {
        var filename = FileExtensions.GetRollingFileName(_filename, _fileIndex);
        return new StreamWriter(filename, true);
    }
    
    private async Task RollFile()
    {
        await _writer.DisposeAsync();
        
        if (_fileIndex >= _maxFileCount)
        {
            _fileIndex = 0;
        }
        else
        {
            _fileIndex++;
        }

        _writer = CreateStreamWriter();
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _loggingTask.Wait();
        _writer.Dispose();
        _cancellationTokenSource.Dispose();
    }
}
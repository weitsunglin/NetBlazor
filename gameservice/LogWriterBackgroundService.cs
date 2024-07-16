using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class LogWriterBackgroundService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly string logFilePath = "Logs/log.txt";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(WriteLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    private void WriteLog(object? state)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? string.Empty);

            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: This is a log entry.");
            }
        }
        catch (Exception)
        {
            // 可以在这里添加异常处理逻辑
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class LogWriterBackgroundService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly string logFilePath = "Logs/log.txt";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // 创建定时器，每秒调用一次 WriteLog
        _timer = new Timer(WriteLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    private void WriteLog(object state)
    {
        try
        {
            // 确保目录存在
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            // 写入日志
            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: This is a log entry.");
            }
        }
        catch (Exception ex)
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

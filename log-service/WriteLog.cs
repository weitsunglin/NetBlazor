using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class WriteLog : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly string logFilePath = "Logs/log.txt";
    private readonly string _serverName;

    public WriteLog(string serverName)
    {
        _serverName = serverName;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(WriteLogEntry, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    private void WriteLogEntry(object? state)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? string.Empty);

            string newLogEntry = $"{DateTime.Now} [{_serverName}]: This is a log entry.{Environment.NewLine}";

            // 讀取現有的日誌內容
            string existingLog = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : string.Empty;

            // 將新日誌條目插入到最前面
            string updatedLog = newLogEntry + existingLog;

            // 寫入更新後的日誌內容
            File.WriteAllText(logFilePath, updatedLog);
        }
        catch (Exception ex)
        {
            // 處理或記錄異常
            Console.WriteLine($"An error occurred: {ex.Message}");
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

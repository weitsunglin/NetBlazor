using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class WriteLog : IHostedService, IDisposable
{
    private readonly string logFilePath = "Logs/log.txt";
    private readonly string _serverName;

    public WriteLog(string serverName)
    {
        _serverName = serverName;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void WriteLogEntry(object? state)
    {
        WriteLogEntry("This is a periodic log entry.");
    }

    public void WriteLogEntry(string message)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? string.Empty);

            string newLogEntry = $"{DateTime.Now} [{_serverName}]: {message}{Environment.NewLine}";

            string existingLog = File.Exists(logFilePath) ? File.ReadAllText(logFilePath) : string.Empty;

            string updatedLog = newLogEntry + existingLog;

            File.WriteAllText(logFilePath, updatedLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}

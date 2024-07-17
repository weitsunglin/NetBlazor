using System.IO;
using System.Threading.Tasks;

public class LogService : ILogService
{
    private readonly string logFilePath = "Logs/log.txt";

    public async Task<string> GetLogContentAsync()
    {
        if (!File.Exists(logFilePath))
        {
            return "Log file not found.";
        }

        using (var reader = new StreamReader(logFilePath))
        {
            return await reader.ReadToEndAsync();
        }
    }
}
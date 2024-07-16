using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddControllers();
builder.Services.AddHostedService<LogWriterBackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5283")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();

public interface ILogService
{
    Task<string> GetLogContentAsync();
}

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

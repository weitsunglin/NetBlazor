using A001Model;
using CommonLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WriteLog>(sp => new WriteLog("a001"));
builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddSingleton<IHostedService, A001TcpNetwork>(sp =>
{
    var writeLog = sp.GetRequiredService<WriteLog>();
    return new A001TcpNetwork(5002, writeLog);
});

builder.WebHost.UseUrls("http://localhost:5000");

Console.WriteLine("WebHost Server is being initialized on http://localhost:5000");

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5283")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

try
{
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
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    throw;
}

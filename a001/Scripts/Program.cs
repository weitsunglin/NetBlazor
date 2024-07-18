var builder = WebApplication.CreateBuilder(args);

// 注册服务
builder.Services.AddSingleton<IHostedService, TcpNetWork>();
builder.WebHost.UseUrls("http://localhost:5000");

Console.WriteLine("WebHost Server is being initialized on http://localhost:5000");

// 创建并注册 WriteLog 服务
var writeLog = new WriteLog("a001");
builder.Services.AddSingleton(writeLog);

builder.Services.AddScoped<ILogService, LogService>();
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
    // 捕获异常并写入日志
    writeLog.WriteLogEntry($"An error occurred: {ex.Message}");
    Console.WriteLine($"An error occurred: {ex.Message}");
    throw; // 重新抛出异常以确保应用程序终止
}

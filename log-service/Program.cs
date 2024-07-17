using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 註冊 TCP Server 服務
builder.Services.AddSingleton<IHostedService, TcpServer>();
builder.WebHost.UseUrls("http://localhost:5000");

// 註冊 WriteLog 服務並傳遞服務器名稱參數
builder.Services.AddSingleton<IHostedService>(provider => new WriteLog("Server1-Prod")); // 替換為實際的服務器名稱

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddControllers();
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

// 配置 HTTP 請求處理路由
app.MapGet("/httpreq", async context =>
{
    await context.Response.WriteAsync("This is an HTTP request response");
});

app.Run();

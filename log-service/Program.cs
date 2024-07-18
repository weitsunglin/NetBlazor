var builder = WebApplication.CreateBuilder(args);

// 註冊 TCP Server 服務
builder.Services.AddSingleton<IHostedService, TcpServer>();
builder.WebHost.UseUrls("http://localhost:5000");

// 註冊 WriteLog 服務並傳遞服務器名稱參數
var writeLog = new WriteLog("Server1-Prod");
builder.Services.AddSingleton<IHostedService>(provider => writeLog);

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

// 添加中間件攔截所有請求並寫入日志
app.Use(async (context, next) =>
{
    writeLog.WriteLogEntry($"Received HTTP {context.Request.Method} request at {context.Request.Path}");
    await next.Invoke();
});

// 配置 HTTP 請求處理路由
app.MapGet("/httpreq", async context =>
{
    await context.Response.WriteAsync("This is an HTTP request response");
});

app.Run();

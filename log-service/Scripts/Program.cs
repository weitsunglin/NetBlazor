var builder = WebApplication.CreateBuilder(args);

// 註冊 TCP Server 服務
builder.Services.AddSingleton<IHostedService, TcpServer>();
builder.WebHost.UseUrls("http://localhost:5000");

// 註冊 WriteLog 服務並傳遞服務器名稱參數
var writeLog = new WriteLog("Server1-Prod");
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

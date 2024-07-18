var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHostedService, TcpNetWork>();
builder.WebHost.UseUrls("http://localhost:5000");

Console.WriteLine("WebHost Server is being initialized on http://localhost:5000");

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
using Freedom.ServerMonitor;
using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.DataBase;
using Freedom.ServerMonitor.Extensions;
using Freedom.ServerMonitor.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVault();
builder.AddLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataBaseContext>();
builder.Services.AddScoped<IServerInfoRepository, DbRepository>();
builder.Services.AddMapper();
builder.Services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
builder.Services.AddStackExchangeRedisCacheRepository();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application started");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

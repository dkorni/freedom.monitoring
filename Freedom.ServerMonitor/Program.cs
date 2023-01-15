using System.Configuration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Freedom.ServerMonitor;
using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.DataBase;
using Freedom.ServerMonitor.Extensions;
using Freedom.ServerMonitor.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataBaseContext>();
builder.Services.AddScoped<IServerInfoRepository, DbRepository>();
builder.AddKeyVault();
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

app.Run();
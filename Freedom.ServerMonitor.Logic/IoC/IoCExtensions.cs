using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Freedom.ServerMonitor.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Freedom.ServerMonitor.Logic.IoC;

public static class IoCExtensions
{
    public static IServiceCollection AddKeyVaultManager(this IServiceCollection builder)
    {
        builder.AddSingleton<IKeyVaultManager, KeyVaultManager>();
        return builder;
    }
    
    public static WebApplicationBuilder AddKeyVault(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            var url = builder.Configuration["KeyVault:VaultUri"];
            var tenantId = builder.Configuration["KeyVault:TenantId"];
            var clientId = builder.Configuration["KeyVault:ClientId"];
            var clientSecret = builder.Configuration["KeyVault:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            builder.Services.AddSingleton(new SecretClient(new Uri(url), credential));
        }
        else
        {
            builder.Services.AddAzureClients(azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
            });
        }
        return builder;
    }
    
    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        Log.Logger = logger;
        builder.Host.UseSerilog();
        return builder;
    }
}
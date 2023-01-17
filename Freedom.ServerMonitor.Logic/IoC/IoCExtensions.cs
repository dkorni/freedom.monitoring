using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Freedom.ServerMonitor.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
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
    
    public static IServiceCollection AddKeyVault(this IServiceCollection builder, bool isDevelopment, IConfiguration configuration)
    {
        if (isDevelopment)
        {
            var url = configuration["KeyVault:VaultUri"];
            var tenantId = configuration["KeyVault:TenantId"];
            var clientId = configuration["KeyVault:ClientId"];
            var clientSecret = configuration["KeyVault:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            builder.AddSingleton(new SecretClient(new Uri(url), credential));
        }
        else
        {
            builder.AddAzureClients(azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddSecretClient(configuration.GetSection("KeyVault"));
            });
        }

        builder.AddSingleton<IKeyVaultManager, KeyVaultManager>();
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
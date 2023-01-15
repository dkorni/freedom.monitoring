using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;

namespace Freedom.ServerMonitor.Extensions;

public static class KeyVaultExtensions
{
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
}
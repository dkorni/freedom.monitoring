using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Freedom.ServerMonitor.Extensions;

public static class RedisServiceCollectionExtensions
{
    public static IServiceCollection AddStackExchangeRedisCacheRepository(this IServiceCollection services)
    {
        services.AddSingleton<IDistributedCache>(p =>
        {
            var options = new RedisCacheOptions();
            var keyVaultManager = p.GetRequiredService<IKeyVaultManager>();
            options.Configuration = keyVaultManager.GetSecret("Freedom-RedisServer");
            options.InstanceName = "Freedom_ServerMonitoring";
            return new RedisCache(options);
        });
        
        services.Decorate<IServerInfoRepository, CacheRepository>();

        return services;
    }
}
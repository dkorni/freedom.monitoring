using Freedom.ServerMonitor.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace Freedom.ServerMonitoring.RedisRepository.IoC;

public static class IoCExtensions
{
    public static IServiceCollection AddRedisCacheRepository(this IServiceCollection services)
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
using Freedom.ServerMonitor.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Freedom.ServerMonitor.DbRepository.IoC;

public static class IoCExtensions
{
    public static IServiceCollection AddDbRepository(this IServiceCollection builder)
    {
        builder.AddDbContext<DataBaseContext>();
        builder.AddScoped<IServerInfoRepository, DbRepository>();
        return builder;
    }
}
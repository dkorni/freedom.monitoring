using AutoMapper;
using Freedom.ServerMonitor.DTO;
using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Extensions;

public static class AutomapperConfigurationExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<ServerInfoModel, ServerInfoDto>();
            cfg.CreateMap<ServerInfoDto, ServerInfoModel>();
        });
        var mapper = configuration.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}
using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IMonitorService
{
    Task<ServerInfoModel[]> GetAll();

    Task<string> CreateServiceAsync(ServerInfoModel server);
    
    Task StayAlive(ServerInfoModel server);
}
using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Contracts;

public interface IMonitorService
{
    Task<ServerInfoModel> GetAll();
    
    Task StayAlive(ServerInfoModel server);
}
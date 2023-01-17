using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IMonitorManagementService
{
    Task<ServerInfoModel[]> GetAll();

    Task StayAlive(ServerInfoModel server);
}
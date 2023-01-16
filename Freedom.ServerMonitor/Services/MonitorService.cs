using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Services;

public class MonitorService : IMonitorService
{
    public Task<ServerInfoModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task StayAlive(ServerInfoModel server)
    {
        throw new NotImplementedException();
    }
}
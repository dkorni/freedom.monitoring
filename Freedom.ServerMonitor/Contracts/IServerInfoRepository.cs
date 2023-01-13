using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Contracts;

public interface IServerInfoRepository
{
    Task<ServerInfoModel[]> GetAll();

    Task Create(ServerInfoModel serverInfo);
}
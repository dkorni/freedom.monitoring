using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IServerInfoRepository
{
    Task<ServerInfoModel[]> GetAll();
    Task<ServerInfoModel> Get(string name, string address, int port);

    Task Create(ServerInfoModel serverInfo);

    Task Update(ServerInfoModel serverInfoModel);
    
    Task UpdateBulk(ServerInfoModel[] serverInfoModel);
}
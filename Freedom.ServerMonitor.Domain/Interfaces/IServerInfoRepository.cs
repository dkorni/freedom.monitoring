using Freedom.ServerMonitor.Domain.Models;

namespace Freedom.ServerMonitor.Domain.Interfaces;

public interface IServerInfoRepository
{
    Task<ServerInfoModel[]> GetAll();
    Task<ServerInfoModel[]> GetAllActive();
    Task<ServerInfoModel> Get(Guid id);

    Task Create(ServerInfoModel serverInfo);

    Task Update(ServerInfoModel serverInfoModel);
    
    Task UpdateBulk(ServerInfoModel[] serverInfoModel);
}
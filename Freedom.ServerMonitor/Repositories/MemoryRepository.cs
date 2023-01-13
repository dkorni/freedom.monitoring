using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Models;

namespace Freedom.ServerMonitor.Repositories;

public class MemoryRepository : IServerInfoRepository
{
    private IDictionary<Guid, ServerInfoModel> _serverInfoModels;

    public MemoryRepository()
    {
        var serverInfoList = new List<ServerInfoModel>()
        {
            new ServerInfoModel
            {
                Id = Guid.NewGuid(),
                Name = "Server only for true cossaks",
                PlayerCount = 24,
                MaxPlayer = 100,
                IpAddress = "128.0.0.1",
                Port = 7777
            },
            new ServerInfoModel
            {
                Id = Guid.NewGuid(),
                Name = "Bobber curva",
                PlayerCount = 3,
                MaxPlayer = 100,
                IpAddress = "128.2.2.8",
                Port = 1488
            }
            ,
            new ServerInfoModel
            {
                Id = Guid.NewGuid(),
                Name = "First Europe server",
                PlayerCount = 52,
                MaxPlayer = 100,
                IpAddress = "192.168.1.1",
                Port = 7777
            }
        };

        _serverInfoModels = new Dictionary<Guid, ServerInfoModel>();
        serverInfoList.ForEach(x=>_serverInfoModels[x.Id] = x);
    }
    
    public Task<ServerInfoModel[]> GetAll()
    {
        return Task.FromResult(_serverInfoModels.Values.ToArray());
    }

    public Task Create(ServerInfoModel serverInfo)
    {
        _serverInfoModels[serverInfo.Id] = serverInfo;
        return  Task.CompletedTask;
    }
}
using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Freedom.ServerMonitoring.RedisRepository.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Freedom.ServerMonitoring.RedisRepository;

public class CacheRepository : IServerInfoRepository
{
    private readonly IDistributedCache _distributedCache;

    public CacheRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _distributedCache.SetRecordAsync(":servers", new List<ServerInfoModel>()
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
            },
            new ServerInfoModel
            {
                Id = Guid.NewGuid(),
                Name = "First Europe server",
                PlayerCount = 52,
                MaxPlayer = 100,
                IpAddress = "192.168.1.1",
                Port = 7777
            }
        }).Wait();
    }
    
    public async Task<ServerInfoModel[]> GetAll()
    {
        var servers = await _distributedCache.GetRecordAsync<ServerInfoModel[]>(":servers");
        return servers;
    }

    public async Task<ServerInfoModel> Get(string name, string address, int port)
    {
        var servers = await _distributedCache.GetRecordAsync<ServerInfoModel[]>(":servers");
        return servers.FirstOrDefault(x=>x.Name == name && x.IpAddress == address && x.Port == port);
    }

    public async Task Create(ServerInfoModel serverInfo)
    {
        var servers = await _distributedCache.GetRecordAsync<List<ServerInfoModel>>(":servers");
        if (servers is null)
        {
            servers = new List<ServerInfoModel>();
        }
        
        serverInfo.CreatedAt = DateTime.UtcNow;
        serverInfo.UpdatedAt = DateTime.UtcNow;
        servers.Add(serverInfo);
        await _distributedCache.SetRecordAsync(":servers", servers);
    }

    public async Task Update(ServerInfoModel serverInfoModel)
    {
        var servers = await _distributedCache.GetRecordAsync<List<ServerInfoModel>>(":servers");
        var serverInfo = servers.FirstOrDefault(x =>
            x.Name == serverInfoModel.Name && x.IpAddress == serverInfoModel.IpAddress &&
            x.Port == serverInfoModel.Port);
        serverInfo.UpdatedAt = DateTime.UtcNow;
        await _distributedCache.SetRecordAsync(":servers", servers);
    }

    public Task UpdateBulk(ServerInfoModel[] servers)
    {
        return _distributedCache.SetRecordAsync(":servers", servers);
    }
}
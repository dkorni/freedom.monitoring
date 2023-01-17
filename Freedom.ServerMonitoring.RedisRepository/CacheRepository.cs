using Freedom.ServerMonitor.Domain.Interfaces;
using Freedom.ServerMonitor.Domain.Models;
using Freedom.ServerMonitoring.RedisRepository.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Freedom.ServerMonitoring.RedisRepository;

public class CacheRepository : IServerInfoRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IServerInfoRepository _decorated;

    private readonly int _expireTime;

    public CacheRepository(IDistributedCache distributedCache, IServerInfoRepository decorated, IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        _decorated = decorated;
        _expireTime = int.Parse(configuration["ExpireTime"]);
    }

    public Task<ServerInfoModel[]> GetAll() => _decorated.GetAll();

    public Task<ServerInfoModel[]> GetAllActive() => GetAllInternal();

    public Task<ServerInfoModel> Get(Guid id) => _decorated.Get(id);

    public async Task Create(ServerInfoModel serverInfo)
    {
       await _decorated.Create(serverInfo);
       await PopulateFromDatabase();
    }

    public async Task Update(ServerInfoModel serverInfoModel)
    {
        await _decorated.Update(serverInfoModel); 
        await PopulateFromDatabase();
    } 

    public async Task UpdateBulk(ServerInfoModel[] servers)
    {
        await _decorated.UpdateBulk(servers);
        await PopulateFromDatabase();
    }

    private async Task<ServerInfoModel[]> GetAllInternal()
    {
        var servers = await _distributedCache.GetRecordAsync<ServerInfoModel[]>(":servers");

        if (servers is null || servers.Length == 0)
        {
            return await PopulateFromDatabase();
        }

        return servers;
    }
    
    private async Task<ServerInfoModel[]> PopulateFromDatabase()
    {
        var servers = await _decorated.GetAllActive();
        await _distributedCache.SetRecordAsync(":servers", servers, TimeSpan.FromSeconds(_expireTime));
        return servers;
    }
}
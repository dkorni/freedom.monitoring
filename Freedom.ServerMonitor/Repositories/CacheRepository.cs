using Freedom.ServerMonitor.Contracts;
using Freedom.ServerMonitor.Extensions;
using Freedom.ServerMonitor.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Freedom.ServerMonitor.Repositories;

public class CacheRepository : IServerInfoRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IServerInfoRepository _decorated;

    public CacheRepository(IDistributedCache distributedCache, IServerInfoRepository decorated)
    {
        _distributedCache = distributedCache;
        _decorated = decorated;
    }
    
    public async Task<ServerInfoModel[]> GetAll()
    {
        var servers = await _distributedCache.GetRecordAsync<ServerInfoModel[]>("ServerInfos");

        if (servers is null)
        {
            return await PopulateFromDatabase();
        }

        return servers;
    }

    public async Task Create(ServerInfoModel serverInfo)
    {
        await _decorated.Create(serverInfo);
        var servers = await _distributedCache.GetRecordAsync<List<ServerInfoModel>>("ServerInfos");
        if (servers is null)
        {
            await PopulateFromDatabase();
            return;
        }
        
        servers.Add(serverInfo);
        await _distributedCache.SetRecordAsync("ServerInfos", servers);
    }

    private async Task<ServerInfoModel[]> PopulateFromDatabase()
    {
        var servers = await _decorated.GetAll();
        await _distributedCache.SetRecordAsync("ServerInfos", servers);
        return servers;
    }
}
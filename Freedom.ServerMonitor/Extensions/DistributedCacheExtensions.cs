using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Freedom.ServerMonitor.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, 
        string recordId, 
        T data,
        TimeSpan? absoluteExpireTime = null, 
        TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions();
        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(15);
        options.SlidingExpiration = unusedExpireTime;
        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        if (jsonData is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
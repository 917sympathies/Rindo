using Application.Interfaces.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Rindo.Infrastructure.Services.Caching;

public class RedisCacheService(IDistributedCache distributedCache): IRedisCacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        
        var data = await distributedCache.GetStringAsync(key);

        return data is null ? default : JsonConvert.DeserializeObject<T>(data);
    }

    public async Task SetAsync<T>(string key, T value)
    {
        ArgumentNullException.ThrowIfNull(key);
        
        var serializeOptions = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        };
        
        await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value, serializeOptions), cacheOptions);
    }

    public async Task RemoveAsync(string key)
    {
        if(string.IsNullOrWhiteSpace(key)) return;
        await distributedCache.RemoveAsync(key);
    }

    public void Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        distributedCache.Remove(key);
    }
}
﻿namespace Rindo.Infrastructure.Interfaces.Caching;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
    Task RemoveAsync(string key);
    void Remove(string key);
}
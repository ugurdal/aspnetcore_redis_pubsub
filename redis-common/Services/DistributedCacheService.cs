using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using redis_common;
using redis_common.Common;
using redis_common.Infrastructure;
using StackExchange.Redis;

namespace redis_common.Services
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public T Get<T>(string key) where T : class
        {
            var value = Get(key);
            return value.ToObject<T>();
        }

        public string Get(string key)
        {
            return Encoding.UTF8.GetString(_distributedCache.Get(key));
        }

        public async Task<string> GetAsync(string key)
        {
            var data = await _distributedCache.GetAsync(key);
            return Encoding.UTF8.GetString(data);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await GetAsync(key);
            return value.ToObject<T>();
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public void Set(string key, string value)
        {
            _distributedCache.SetString(key, value);
        }

        public void Set<T>(string key, T value) where T : class
        {
            var data = Encoding.UTF8.GetBytes(value.FromJson());
            _distributedCache.Set(key, data);
        }

        public void Set(string key, object value, DistributedCacheEntryOptions cacheEntryOptions)
        {
            var data = Encoding.UTF8.GetBytes(value.FromJson());
            _distributedCache.Set(key, data, cacheEntryOptions);
        }

        public async Task SetAsync(string key, object value)
        {
            var data = Encoding.UTF8.GetBytes(value.FromJson());
            await _distributedCache.SetAsync(key, data);
        }

        public async Task SetAsync(string key, object value, DistributedCacheEntryOptions cacheEntryOptions)
        {
            var data = Encoding.UTF8.GetBytes(value.FromJson());
            await _distributedCache.SetAsync(key, data, cacheEntryOptions);
        }
    }
}
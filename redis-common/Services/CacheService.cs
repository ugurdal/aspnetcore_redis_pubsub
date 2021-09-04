using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using redis_common;
using redis_common.Common;
using redis_common.Infrastructure;
using StackExchange.Redis;

namespace redis_common.Services
{
    public class CacheService : ICacheService
    {
        private readonly RedisConfig _redisConfig;
        private readonly ConnectionMultiplexer _redisClient;

        public CacheService(IConfiguration configuration)
        {
            _redisConfig = configuration.GetSection("Redis").Get<RedisConfig>();
            if (_redisConfig == null)
                throw new NotImplementedException("Redis is null");

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { _redisConfig.ConnectionString },
                AbortOnConnectFail = false,
                AsyncTimeout = 10_000,
                ConnectTimeout = 10_000
            };

            _redisClient = ConnectionMultiplexer.Connect(configurationOptions);
        }

        public T Get<T>(string key) where T : class
        {
            var value = Get(key);
            return value.ToObject<T>();
        }

        public string Get(string key)
        {
            return _redisClient.GetDatabase().StringGet(key);
        }

        public async Task<string> GetAsync(string key)
        {
            return await _redisClient.GetDatabase().StringGetAsync(key);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await GetAsync(key);
            return value.ToObject<T>();
        }

        public void Remove(string key)
        {
            _redisClient.GetDatabase().KeyDelete(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _redisClient.GetDatabase().KeyDeleteAsync(key);
        }

        public void Set(string key, string value)
        {
            _redisClient.GetDatabase().StringSet(key, value);
        }

        public void Set<T>(string key, T value) where T : class
        {
            _redisClient.GetDatabase().StringSet(key, value.FromJson());
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            _redisClient.GetDatabase().StringSet(key, value.FromJson(), expiration);
        }

        public async Task SetAsync(string key, object value)
        {
            await _redisClient.GetDatabase().StringSetAsync(key, value.FromJson());
        }

        public async Task SetAsync(string key, object value, TimeSpan expiration)
        {
            await _redisClient.GetDatabase().StringSetAsync(key, value.FromJson(), expiration);
        }
    }
}
using System;
using System.Threading.Tasks;

namespace redis_common.Infrastructure
{
    public interface ICacheService
    {
        void Set(string key, string value);
        void Set<T>(string key, T value) where T : class;
        Task SetAsync(string key, object value);
        void Set(string key, object value, TimeSpan expiration);
        Task SetAsync(string key, object value, TimeSpan expiration);
        T Get<T>(string key) where T : class;
        Task<T> GetAsync<T>(string key) where T : class;
        string Get(string key);
        Task<string> GetAsync(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
    }
}
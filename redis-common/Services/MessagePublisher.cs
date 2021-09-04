using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using redis_common;
using redis_common.Common;
using redis_common.Infrastructure;
using redis_common.Models;
using StackExchange.Redis;

namespace redis_common.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly RedisConfig _redisConfig;
        private readonly ConnectionMultiplexer _redisClient;

        public MessagePublisher(IConfiguration configuration)
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

        public async Task UserCreatedAsync(UserDto user)
        {
            await _redisClient.GetDatabase().PublishAsync(Core.UserChannel, user.FromJson());
        }
    }
}
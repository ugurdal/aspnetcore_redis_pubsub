using System;
using System.Threading.Tasks;
using redis_common;
using redis_common.Common;
using redis_common.Models;
using StackExchange.Redis;

namespace redis_subscriber
{
    class Program
    {
        private static void Main(string[] args)
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "localhost:6379" },
                AbortOnConnectFail = false,
                AsyncTimeout = 10_000,
                ConnectTimeout = 10_000
            };

            var redisClient = ConnectionMultiplexer.Connect(configurationOptions);
            var pubsub = redisClient.GetSubscriber();

            pubsub.Subscribe(Core.UserChannel, (channel, message) =>
            {
                Console.WriteLine(message.ToString().ToObject<UserDto>());
            });


            Console.ReadLine();
        }
    }
}

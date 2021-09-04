namespace redis_common
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public int DefaultExpirationMilis { get; set; }
    }
}
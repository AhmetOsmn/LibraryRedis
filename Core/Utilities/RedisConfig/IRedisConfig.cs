using StackExchange.Redis;

namespace Core.Utilities.RedisConfig
{
    public interface IRedisConfig
    {
        ConnectionMultiplexer Connection();
    }
}

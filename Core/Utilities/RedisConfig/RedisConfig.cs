using StackExchange.Redis;
using System;

namespace Core.Utilities.RedisConfig
{
    public class RedisConfig : IRedisConfig
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;


        public RedisConfig()
        {
            try
            {
                _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost:6379, abortConnect=false"));
            }
            catch (Exception)
            {

                Console.WriteLine("redis sunucusu aktif değil.");
            }
        }

        public ConnectionMultiplexer Connection()
        {
            return _connection.Value;
        }
    }
}

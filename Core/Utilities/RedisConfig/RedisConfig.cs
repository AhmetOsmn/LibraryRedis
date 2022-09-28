using StackExchange.Redis;
using System;

namespace Core.Utilities.RedisConfig
{
    public class RedisConfig : IRedisConfig
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;


        public RedisConfig()
        {
            //_connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost:6379, abortConnect=false"));
            var options = ConfigurationOptions.Parse("localhost:6379, abortConnect=false");
            options.AllowAdmin = true;
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        public ConnectionMultiplexer Connection()
        {
            return _connection.Value;
        }
    }
}

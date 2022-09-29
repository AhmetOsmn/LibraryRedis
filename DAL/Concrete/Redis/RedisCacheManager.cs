using Core.Utilities.RedisConfig;
using DAL.Abstract;
using Newtonsoft.Json;
using ServiceStack;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace DAL.Concrete.Redis
{
    public class RedisCacheManager : IRedisCacheService
    {
        private readonly IRedisConfig _connection;
        private readonly IDatabase _database;


        public RedisCacheManager(IRedisConfig connection)
        {
            _connection = connection;
            // 0. db
            _database = _connection.Connection().GetDatabase();
            //Clear();
        }


        public bool IsConnected()
        {
            return _connection.Connection().IsConnected;
        }

        public void Add(string key, object value)
        {
            string jsonData = JsonConvert.SerializeObject(value);

            
            _database.StringSet(key, jsonData);
        }

        public void Add(string key, object value, DateTime expireTime)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            TimeSpan expire = new TimeSpan((expireTime.Day-DateTime.Now.Day), (expireTime.Minute - DateTime.Now.Minute), (expireTime.Second - DateTime.Now.Second));
            _database.StringSet(key, jsonData, expire);
        }

        public bool Any(string key)
        {
            return _database.KeyExists(key);
        }

        public void Clear()
        {
            //_database.Multiplexer.GetServer("http:localhost:6379").FlushDatabase();
            var endpoints = _database.Multiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _database.Multiplexer.GetServer(endpoint);
                server.FlushDatabase(1);
            }
        }

        public T Get<T>(string key)
        {
            if (!Any(key)) return default;

            string jsonData = _database.StringGet(key);

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public List<T> GetAll<T>(string key)
        {
            List<T> list = new List<T>();

            RedisResult keyList = _database.Execute("keys",key);

            foreach (var item in ((RedisKey[]?)keyList))
            {
                string tempKey = item.ToString().Replace("{","").Replace("}","");
                list.Add(JsonConvert.DeserializeObject<T>(_database.StringGet(tempKey)));
            }

            return list;
        }

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }
    }
}

using System;
using System.Collections.Generic;

namespace DAL.Abstract
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);
        List<T> GetAll<T>(string key);
        void Add(string key, object value);
        void Add(string key, object value, DateTime expireTime);
        void Remove(string key);
        void Clear();
        bool Any(string key);
    }
}

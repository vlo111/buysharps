using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace buysharps.Helpers
{
    public class CacheManager
    {
        private const string _CacheKey = "CacheKey";

        public static object List
        {
            get
            {
                return GetValue(_CacheKey);
            }
            set
            {
                Update(_CacheKey, value);
            }
        }
        public static object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }
        public static void Update(string key, object value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            if (value != null)
            {
                memoryCache.Set(key, value, policy);
            }
            else
            {
                Delete(key);
            }
        }
        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemcachedProviders.Cache;

namespace NETCommon
{
    public class MemoryCache
    {
        public static bool Add(string key, object value)
        {
            return DistCache.Add(key, value);
        }

        public static bool Add(string key, object value, DateTimeOffset dtsOffset)
        {
            return DistCache.Add(key, value, dtsOffset.Second);
        }

        public static object Get(string key)
        {
            return DistCache.Get(key);
        }

        public static object Remove(string key)
        {
            return DistCache.Remove(key);
        }
    }
}

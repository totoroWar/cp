using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCommon
{
    /// <summary>
    /// 实现WEB、WINFORM，总之是.NET库层中的缓存，非只针对WEB的单一无扩展方式
    /// </summary>
    public class GeneralCache
    {
        public static void Set(string key, object value, DateTimeOffset offset)
        {
            //System.Runtime.Caching.MemoryCache.Default.Set(key + "_" + MD5.Get(key, MD5Bit.L32) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), value, offset);
            System.Runtime.Caching.MemoryCache.Default.Set(key, value, offset);
        }

        public static void Set(string key, object value)
        {
            int defaultTime = ConfigHelper.GetConfigInt("CJLSoftNormalCacheTime");
            defaultTime = defaultTime == 0 ? 60 : defaultTime;
            //System.Runtime.Caching.MemoryCache.Default.Set(key + "_" + MD5.Get(key, MD5Bit.L32)+"_"+DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), value, DateTimeOffset.Now.AddMinutes(defaultTime));
            System.Runtime.Caching.MemoryCache.Default.Set(key, value, DateTimeOffset.Now.AddMinutes(defaultTime));
        }

        public static object Get(string key)
        {
            return System.Runtime.Caching.MemoryCache.Default.Get(key);
        }

        public static void Clear(string key)
        {
            object isRemove = System.Runtime.Caching.MemoryCache.Default.Remove(key);
        }

        public static void ClearLike(string key)
        {
            var list = GetCacheList().Where(exp=>exp.Key.Contains(key)).ToList();
            foreach (var item in list)
            {
                System.Runtime.Caching.MemoryCache.Default.Remove(item.Key);
            }
        }

        public static List<KeyValuePair<string, object>> GetCacheList()
        {
            List<KeyValuePair<string,object>> list = System.Runtime.Caching.MemoryCache.Default.ToList();
            return list;
        }
    }


}

namespace MStech.Infrastructure.Caching.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Threading;

    public partial class MemoryCacheManager : ICacheManager
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        private static readonly Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// Armazena as chaves do cache("cache" de chaves) para otimizar a busca por chave do método RemoveByPattern
        /// </summary>
        private static readonly System.Collections.Generic.List<string> cacheKeys = new List<string>();
        private static readonly object _lockObject = new object();
             
        public static void AddCacheKey(string key)
        {
            lock (_lockObject)
            {
                cacheKeys.Add(key);
            }
            
        }
        private static void RemoveCacheKey(string key)
        {
            lock (_lockObject)
            {
                cacheKeys.Remove(key);
            }
        }

        
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }


        public string GetString(string key)
        {
            var valueBytes = Cache.Get(key) as byte[];
            if (valueBytes == null)
            {
                return String.Empty;
            }

            var sString = encoding.GetString(valueBytes);

            return sString;
        }

        public T Get<T>(string key, Func<T> acquire, int cacheTime = 60)
        {
            if (this.IsSet(key))
            {
                return this.Get<T>(key);
            }
            else
            {
                T result = acquire();
                if (cacheTime > 0)
                    this.Set(key, result, cacheTime);
                return result;
            }
        }

        public T GetFromString<T>(string key) 
        {
            throw new NotImplementedException();
        }

        public T GetFromString<T>(string key, Func<T> acquire, int cacheTime = 60)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime = 60)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);

            AddCacheKey(key);
            
        }

      

        public void SetString(string key, object data, int cacheTime = 60)
        {
            if (data == null)
                return;

            var entryBytes = encoding.GetBytes((string)data);

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, entryBytes), policy);
            AddCacheKey(key);
        }

        public void SetToString<T>(string key, T data, int cacheTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
            RemoveCacheKey(key);
        }




        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {                         
           var keysToRemove = cacheKeys.FindAll(p => p.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) != -1);
           foreach (string key in keysToRemove)
           {
                Remove(key);
           }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }


        public Dictionary<string, object> GetAllKey()
        {
            var chaves = new Dictionary<string, object>();
            foreach (var item in Cache)
            {
                chaves.Add(item.Key, item.Value);
            }
            return chaves;
        }

    }
}

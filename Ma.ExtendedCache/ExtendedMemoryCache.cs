using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace Ma.ExtendedCache
{
    /// <summary>
    /// Caching class which extends memory cache.
    /// </summary>
    public class ExtendedMemoryCache
        : MemoryCache
    {
        public ExtendedMemoryCache(string name, NameValueCollection config = null)
            : base(name, config)
        {            
        }

        private static ObjectCache cache = Default;

        /// <summary>
        /// If value already exists in cache get it,
        /// otherwise get value using provided function
        /// and add it to cache for further usage.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// When key or cacheSourceGetter is null
        /// </exception>
        /// <typeparam name="T">Typeof cache item.</typeparam>
        /// <param name="key">Key for cache.</param>
        /// <param name="cacheSourceGetter">Function to get source if needed.</param>
        /// <param name="keepHour">How many hours should this item be keeped in cahce.</param>
        /// <returns>Item from cache or from source.</returns>
        public static T Retrieve<T>(
            string key, 
            Func<T> cacheSourceGetter,
            int keepHour)
            where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (cacheSourceGetter == null)
                throw new ArgumentNullException("cacheSourceGetter");

            if (cache.Contains(key))
            {
                return cache.Get(key) as T;
            }
            else
            {
                // Get value from source
                T value = cacheSourceGetter.Invoke();

                CacheItem cahceItem = new CacheItem(key, value);
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration =
                    new DateTimeOffset(DateTime.Now.Add(TimeSpan.FromHours(keepHour)));

                cache.Add(cahceItem, policy);

                return value;
            }
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Ma.ExtendedCache
{
  /// <inheritdoc />
  public class ExtendedMemoryCache
      : MemoryCache, IExtendedCache
  {
    public ExtendedMemoryCache(string name, NameValueCollection config = null)
        : base(name, config)
    {
    }

    private ObjectCache _cache = Default;

    public T Retrieve<T>(
        string key,
        Func<T> retreieveFromSource,
        int keepHour)
        where T : class
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentNullException("key");
      if (retreieveFromSource == null)
        throw new ArgumentNullException("retreieveFromSource");

      if (_cache.Contains(key))
      {
        return this.RetreieveFromCache<T>(key);
      }
      else
      {
        // Get value from source
        var value = retreieveFromSource();

        this.AddToCache<T>(key, value, keepHour);

        return value;
      }
    }

    public async Task<T> RetrieveAsync<T>(
      string key,
      Func<Task<T>> retreieveFromSourceAsync,
      int keepHour)
      where T : class
    {
      if (string.IsNullOrEmpty(key))
        throw new ArgumentNullException(nameof(key));
      if (retreieveFromSourceAsync == null)
        throw new ArgumentNullException(nameof(retreieveFromSourceAsync));

      if (_cache.Contains(key))
      {
        return this.RetreieveFromCache<T>(key);
      }
      else
      {
        // Get value from source
        var value = await retreieveFromSourceAsync();

        this.AddToCache<T>(key, value, keepHour);

        return value;
      }
    }

    private T RetreieveFromCache<T>(string key)
      where T : class
    {
      if (_cache.Contains(key) == false)
        return null;

      return _cache.Get(key) as T;
    }

    private void AddToCache<T>(string key, T value, int keepHour)
      where T : class
    {
      var cahceItem = new CacheItem(key, value);
      var policy = new CacheItemPolicy
      {
        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.Add(TimeSpan.FromHours(keepHour)))
      };

      _cache.Add(cahceItem, policy);
    }
  }
}

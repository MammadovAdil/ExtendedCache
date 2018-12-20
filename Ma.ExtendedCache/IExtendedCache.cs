using System;
using System.Threading.Tasks;

namespace Ma.ExtendedCache
{
  public interface IExtendedCache
  {
    /// <summary>
    /// If value already exists in cache get it,
    /// otherwise get value using provided function
    /// and add it to cache for further usage.
    /// </summary>
    /// <typeparam name="T">Typeof cache item.</typeparam>
    /// <param name="key">Key for cache.</param>
    /// <param name="retreieveFromSource">Function to get source if needed.</param>
    /// <param name="keepHour">How many hours should this item be keeped in cahce.</param>
    /// <returns>Item from cache or from source.</returns>
    T Retrieve<T>(string key, Func<T> retreieveFromSource, int keepHour)
        where T : class;

    /// <summary>
    /// If value already exists in cache get it,
    /// otherwise get value using provided function
    /// and add it to cache for further usage.
    /// </summary>
    /// <typeparam name="T">Typeof cache item.</typeparam>
    /// <param name="key">Key for cache.</param>
    /// <param name="retreieveFromSourceAsync">Function to get source asynchronously if needed.</param>
    /// <param name="keepHour">How many hours should this item be keeped in cahce.</param>
    /// <returns>Item from cache or from source.</returns>
    Task<T> RetrieveAsync<T>(string key, Func<Task<T>> retreieveFromSourceAsync, int keepHour)
        where T : class;
  }
}

using Microsoft.Extensions.Caching.Memory;

namespace PeopleOps.Web.Extensions;

public static class MemoryCacheExtensions
{
    public static T? GetOrCreate<T>(this IMemoryCache cache, string key, Func<T> createItem)
    {
        if (!cache.TryGetValue(key, out T? cacheEntry))
        {
            cacheEntry = createItem();
            cache.Set(key, cacheEntry);
        }

        return cacheEntry;
    }
    
    public static T? GetOrCreate<T>(this IMemoryCache cache, string key, Func<T> createItem, TimeSpan expiration)
    {
        if (!cache.TryGetValue(key, out T? cacheEntry))
        {
            cacheEntry = createItem();
            cache.Set(key, cacheEntry, expiration);
        }

        return cacheEntry;
    }
    
    public static T? GetOrCreate<T>(this IMemoryCache cache, string key, Func<T> createItem, MemoryCacheEntryOptions options)
    {
        if (!cache.TryGetValue(key, out T? cacheEntry))
        {
            cacheEntry = createItem();
            cache.Set(key, cacheEntry, options);
        }

        return cacheEntry;
    }
    
    public static T? GetOrCreate<T>(this IMemoryCache cache, string key, Func<T> createItem, DateTimeOffset absoluteExpiration)
    {
        if (!cache.TryGetValue(key, out T? cacheEntry))
        {
            cacheEntry = createItem();
            cache.Set(key, cacheEntry, absoluteExpiration);
        }

        return cacheEntry;
    }
    
    public static T? GetOrCreate<T>(this IMemoryCache cache, string key, Func<T> createItem, TimeSpan slidingExpiration, DateTimeOffset absoluteExpiration)
    {
        if (!cache.TryGetValue(key, out T? cacheEntry))
        {
            cacheEntry = createItem();
            cache.Set(key, cacheEntry, new MemoryCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration,
                AbsoluteExpiration = absoluteExpiration
            });
        }

        return cacheEntry;
    }
    
    //get IMemoryCache from HttpContext
    public static IMemoryCache GetMemoryCache(this HttpContext context)
    {
        return context.RequestServices.GetRequiredService<IMemoryCache>();
    }
    
}
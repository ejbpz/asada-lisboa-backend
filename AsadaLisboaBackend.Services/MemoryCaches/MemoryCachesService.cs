using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.MemoryCaches;

public class MemoryCachesService(IMemoryCache memoryCache) : IMemoryCachesService
{
    public async Task<T> GetOrCreateCache<T>(string key, Func<Task<T>> create, TimeSpan time)
    {
        if (memoryCache.TryGetValue(key, out T? element))
            return element!;

        element = await create();

        memoryCache.Set(key, element, time);
        return element;
    }

    public async Task<T> GetOrCreateCacheList<T>(string resource, object request, Func<Task<T>> create, TimeSpan time)
    {
        var version = GetVersion(resource);
        var hash = HashGenerator(request);

        var key = $"{resource}_list_{version}_{hash}";

        return await GetOrCreateCache(key, create, time);
    }

    public void RemoveById(string resource, object id)
    {
        var key = $"{resource}_{id}";
        memoryCache.Remove(key);
    }

    public void ChangeVersion(string resource)
    {
        var versionKey = GetVersionedKey(resource);
        memoryCache.Set(versionKey, Guid.NewGuid().ToString());
    }

    private string GetVersion(string resource)
    {
        var key = GetVersionedKey(resource);

        if(!memoryCache.TryGetValue(key, out string? version))
        {
            version = Guid.NewGuid().ToString();
            memoryCache.Set(key, version);
        }

        return version!;
    }

    private string GetVersionedKey(string resource) => $"{resource}_cache_version";

    private string HashGenerator(object request)
    {
        if (request is string str)
            return NormalizeString(str);

        var json = JsonSerializer.Serialize(request);

        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(json));

        return Convert.ToHexString(hashBytes);
    }

    private string NormalizeString(string input)
    {
        return input
            .Trim()
            .ToLowerInvariant();
    }
}
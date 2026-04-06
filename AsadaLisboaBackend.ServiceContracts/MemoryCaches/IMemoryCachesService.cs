namespace AsadaLisboaBackend.ServiceContracts.MemoryCaches
{
    public interface IMemoryCachesService
    {
        Task<T> GetOrCreateCache<T>(string key, Func<Task<T>> create, TimeSpan time);
        Task<T> GetOrCreateCacheList<T>(string resource, object request, Func<Task<T>> create, TimeSpan time);
        void Remove(string key);
        void RemoveById(string resource, object id);
        void ChangeVersion(string resource);
    }
}

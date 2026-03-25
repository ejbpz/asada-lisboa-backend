using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsDeleterService
    {
        public Task DeleteNew(Guid id, FileStorageOptions options);
    }
}

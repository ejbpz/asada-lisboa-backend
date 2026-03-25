using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsDeleterService : INewsDeleterService
    {
        private readonly INewsDeleterRepository _newsDeleterRepository;

        public NewsDeleterService(INewsDeleterRepository newsDeleterRepository)
        {
            _newsDeleterRepository = newsDeleterRepository;
        }

        public async Task DeleteNew(Guid id, FileStorageOptions options)
        {
            // TODO: Delete related images

            await _newsDeleterRepository.DeleteNew(id);
        }
    }
}

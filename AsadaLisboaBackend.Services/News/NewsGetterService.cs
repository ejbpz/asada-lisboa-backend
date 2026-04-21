using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsGetterService : INewsGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly INewsGetterRepository _newsGetterRepository;

        public NewsGetterService(INewsGetterRepository newsGetterRepository, IMemoryCachesService memoryCachesService)
        {
            _memoryCachesService = memoryCachesService;
            _newsGetterRepository = newsGetterRepository;
        }

        public async Task<NewResponseDTO> GetNew(Guid id)
        {
            return await _memoryCachesService.GetOrCreateCache<NewResponseDTO>(
                 key: $"{Constants.CACHE_NEWS}_{id}",
                 create: () => _newsGetterRepository.GetNew(id),
                 time: TimeSpan.FromMinutes(5));
        }

        public async Task<NewResponseDTO> GetNewBySlug(string slug)
        {
            return await _memoryCachesService.GetOrCreateCache<NewResponseDTO>(
                key: $"{Constants.CACHE_NEWS}_{slug}",
                create: () => _newsGetterRepository.GetNewBySlug(slug),
                time: TimeSpan.FromMinutes(5));
        }

        public async Task<PageResponseDTO<NewMinimalResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO)
        {
            return await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<NewMinimalResponseDTO>>(
                resource: Constants.CACHE_NEWS,
                request: searchSortRequestDTO,
                create: () => _newsGetterRepository.GetNews(searchSortRequestDTO),
                time: TimeSpan.FromMinutes(5));
        }
    }
}

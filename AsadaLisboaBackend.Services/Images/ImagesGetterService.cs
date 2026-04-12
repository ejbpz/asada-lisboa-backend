using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesGetterService : IImagesGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IImagesGetterRepository _imagesGetterRepository;

        public ImagesGetterService(IImagesGetterRepository imagesGetterRepository, IMemoryCachesService memoryCachesService)
        {
            _memoryCachesService = memoryCachesService;
            _imagesGetterRepository = imagesGetterRepository;
        }

        public async Task<PageResponseDTO<ImageMinimalResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _memoryCachesService.GetOrCreateCacheList(
                resource: Constants.CACHE_IMAGES,
                request: searchSortRequestDTO,
                create: () => _imagesGetterRepository.GetImages(searchSortRequestDTO),
                time: TimeSpan.FromMinutes(5));
        }

        public async Task<ImageResponseDTO> GetImage(Guid id)
        {
            return (await _memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_IMAGES}_{id}",
                create: () => _imagesGetterRepository.GetImage(id),
                time: TimeSpan.FromMinutes(5)))
                    .ToImageResponseDTO();
        }

        public async Task<ImageResponseDTO> GetImageBySlug(string slug)
        {
            return (await _memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_IMAGES}_{slug}",
                create: () => _imagesGetterRepository.GetImageBySlug(slug),
                time: TimeSpan.FromMinutes(5)))
                    .ToImageResponseDTO();
        }
    }
}

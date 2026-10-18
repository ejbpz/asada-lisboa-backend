using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images;

public class ImagesGetterService(
    IImagesGetterRepository imagesGetterRepository,
    IMemoryCachesService memoryCachesService)
    : IImagesGetterService
{
    public async Task<PageResponseDto<ImageMinimalResponseDto>> GetImages(SearchSortRequestDto searchSortRequestDto)
    {
        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_IMAGES,
            request: searchSortRequestDto,
            create: () => imagesGetterRepository.GetImages(searchSortRequestDto),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<ImageResponseDto> GetImage(Guid id)
    {
        return (await memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_IMAGES}_{id}",
                create: () => imagesGetterRepository.GetImage(id),
                time: TimeSpan.FromMinutes(5)))
            .ToImageResponseDto();
    }

    public async Task<ImageResponseDto> GetImageBySlug(string slug)
    {
        return (await memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_IMAGES}_{slug}",
                create: () => imagesGetterRepository.GetImageBySlug(slug),
                time: TimeSpan.FromMinutes(5)))
            .ToImageResponseDto();
    }
}
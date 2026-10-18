using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News;

public class NewsGetterService(
    INewsGetterRepository newsGetterRepository, 
    IMemoryCachesService memoryCachesService)
    : INewsGetterService
{
    public async Task<NewResponseDto> GetNew(Guid id)
    {
        return await memoryCachesService.GetOrCreateCache(
            key: $"{Constants.CACHE_NEWS}_{id}",
            create: () => newsGetterRepository.GetNew(id),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<NewResponseDto> GetNewBySlug(string slug)
    {
        return await memoryCachesService.GetOrCreateCache(
            key: $"{Constants.CACHE_NEWS}_{slug}",
            create: () => newsGetterRepository.GetNewBySlug(slug),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<PageResponseDto<NewMinimalResponseDto>> GetNews(SearchSortRequestDto searchSortRequestDto)
    {
        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_NEWS,
            request: searchSortRequestDto,
            create: () => newsGetterRepository.GetNews(searchSortRequestDto),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<List<NewMinimalResponseDto>> GetRecommendedNews(string slug)
    {
        return await memoryCachesService.GetOrCreateCacheList(
            resource: $"{Constants.CACHE_NEWS}-{slug}",
            request: slug,
            create: () => newsGetterRepository.GetRecommendedNews(slug),
            time: TimeSpan.FromMinutes(5));
    }
}
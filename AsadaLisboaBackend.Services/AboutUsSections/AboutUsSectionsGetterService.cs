using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections;

public class AboutUsSectionsGetterService(
    IAboutUsSectionsGetterRepository aboutUsSectionsGetterRepository,
    ILogger<AboutUsSectionsGetterService> logger,
    IMemoryCachesService memoryCachesService)
    : IAboutUsSectionsGetterService
{
    public async Task<PageResponseDto<AboutUsResponseDto>> GetAboutUsSections(SearchSortRequestDto searchSortRequestDto)
    {
        try
        {
            var result = await memoryCachesService.GetOrCreateCacheList<PageResponseDto<AboutUsResponseDto>>(
                resource: Constants.CACHE_ABOUT_US,
                request: searchSortRequestDto,
                create: () => aboutUsSectionsGetterRepository.GetAboutUsSections(searchSortRequestDto),
                time: TimeSpan.FromMinutes(5));

            logger.LogInformation(
                "Obtención exitosa de AboutUsSections. Tamaño: {Take}",
                searchSortRequestDto.Take
            );

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error al obtener AboutUsSections. Tamaño: {Take}",
                searchSortRequestDto.Take
            );
            throw;
        }
    }
}
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections;

public class AboutUsSectionsUpdaterService(
    IAboutUsSectionsUpdaterRepository aboutUsSectionsUpdaterRepository,
    ILogger<AboutUsSectionsUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IAboutUsSectionsUpdaterService
{
    public async Task<AboutUsResponseDto> UpdateAboutUsSection(Guid id, AboutUsRequestDto aboutUsRequestDto)
    {
        var result = await aboutUsSectionsUpdaterRepository.UpdateAboutUsSection(id, aboutUsRequestDto);

        logger.LogInformation("Actualización exitosa de AboutUsSection con Id: {Id}", id);

        memoryCachesService.RemoveById(Constants.CACHE_ABOUT_US, id);
        memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

        return result.ToAboutUsResponseDto();
            
    }
}
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections;

public class AboutUsSectionsAdderService(
    IAboutUsSectionsAdderRepository aboutUsSectionsAdderRepository,
    ILogger<AboutUsSectionsAdderService> logger,
    IMemoryCachesService memoryCachesService) 
    : IAboutUsSectionsAdderService
{
    public async Task<AboutUsResponseDto> CreateAboutUsSection(AboutUsRequestDto aboutUsRequestDto)
    {
        var aboutUsSection = new AboutUsSection()
        {
            Order = aboutUsRequestDto.Order,
            Content = aboutUsRequestDto.Content,
            SectionType = aboutUsRequestDto.SectionType,
        };

        var aboutUsCreate = await aboutUsSectionsAdderRepository.CreateAboutUsSection(aboutUsSection);

        logger.LogInformation("Sección \"sobre nosotros\" creada");
        memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

        return aboutUsCreate.ToAboutUsResponseDto();

    }
}
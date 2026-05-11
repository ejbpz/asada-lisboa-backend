using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsAdderService : IAboutUsSectionsAdderService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ILogger<AboutUsSectionsAdderService> _logger;
        private readonly IAboutUsSectionsAdderRepository _aboutUsSectionsAdderRepository;

        public AboutUsSectionsAdderService(IAboutUsSectionsAdderRepository aboutUsSectionsAdderRepository, ILogger<AboutUsSectionsAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _aboutUsSectionsAdderRepository = aboutUsSectionsAdderRepository;
        }

        public async Task<AboutUsResponseDTO> CreateAboutUsSection(AboutUsRequestDTO aboutUsRequestDTO)
        {

            var aboutUsSection = new AboutUsSection()
            {
                Order = aboutUsRequestDTO.Order,
                Content = aboutUsRequestDTO.Content,
                SectionType = aboutUsRequestDTO.SectionType,
            };

            var aboutUsCreate = await _aboutUsSectionsAdderRepository.CreateAboutUsSection(aboutUsSection);

            _logger.LogInformation("Sección \"sobre nosotros\" creada");

            _memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

            return aboutUsCreate.ToAboutUsResponseDTO();

        }
    }
}

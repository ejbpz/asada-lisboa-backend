using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsAdderService : IAboutUsSectionsAdderService
    {
        private readonly IAboutUsSectionsAdderRepository _aboutUsSectionsAdderRepository;
        private readonly ILogger<AboutUsSectionsAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public AboutUsSectionsAdderService(IAboutUsSectionsAdderRepository aboutUsSectionsAdderRepository, ILogger<AboutUsSectionsAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _aboutUsSectionsAdderRepository = aboutUsSectionsAdderRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
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

            _logger.LogInformation("Sobre nosotros creada");

            _memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

            return aboutUsCreate.ToAboutUsResponseDTO();

        }
    }
}

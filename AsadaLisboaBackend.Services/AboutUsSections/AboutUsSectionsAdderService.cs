using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsAdderService : IAboutUsSectionsAdderService
    {
        private readonly IAboutUsSectionsAdderRepository _aboutUsSectionsAdderRepository;
        private readonly ILogger<AboutUsSectionsAdderService> _logger;

        public AboutUsSectionsAdderService(IAboutUsSectionsAdderRepository aboutUsSectionsAdderRepository, ILogger<AboutUsSectionsAdderService> logger)
        {
            _aboutUsSectionsAdderRepository = aboutUsSectionsAdderRepository;
            _logger = logger;
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

            return aboutUsCreate.ToAboutUsResponseDTO();

        }
    }
}

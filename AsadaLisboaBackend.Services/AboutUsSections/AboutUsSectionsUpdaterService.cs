using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsUpdaterService : IAboutUsSectionsUpdaterService
    {
        private readonly IAboutUsSectionsUpdaterRepository _aboutUsSectionsUpdaterRepository;
        private readonly ILogger<AboutUsSectionsUpdaterService> _logger;

        public AboutUsSectionsUpdaterService(IAboutUsSectionsUpdaterRepository aboutUsSectionsUpdaterRepository, ILogger<AboutUsSectionsUpdaterService> logger)
        {
            _aboutUsSectionsUpdaterRepository = aboutUsSectionsUpdaterRepository;
            _logger = logger;
        }

        public async Task<AboutUsResponseDTO> UpdateAboutUsSection(Guid id, AboutUsRequestDTO aboutUsRequestDTO)
        {
            var result = await _aboutUsSectionsUpdaterRepository.UpdateAboutUsSection(id, aboutUsRequestDTO);

            if (result = null)
            {
                _logger.LogWarning("No se encontró AboutUsSection para actualizar con Id: {Id}", id);
            }

            _logger.LogInformation("Actualización exitosa de AboutUsSection con Id: {Id}", id);

            return result.ToAboutUsResponseDTO();
        }
    }
}

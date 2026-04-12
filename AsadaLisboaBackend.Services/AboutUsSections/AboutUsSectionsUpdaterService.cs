using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

namespace AsadaLisboaBackend.Services.AboutUsSections
{
    public class AboutUsSectionsUpdaterService : IAboutUsSectionsUpdaterService
    {
        private readonly IAboutUsSectionsUpdaterRepository _aboutUsSectionsUpdaterRepository;
        private readonly ILogger<AboutUsSectionsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public AboutUsSectionsUpdaterService(IAboutUsSectionsUpdaterRepository aboutUsSectionsUpdaterRepository, ILogger<AboutUsSectionsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _aboutUsSectionsUpdaterRepository = aboutUsSectionsUpdaterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<AboutUsResponseDTO> UpdateAboutUsSection(Guid id, AboutUsRequestDTO aboutUsRequestDTO)
        {
            var result = await _aboutUsSectionsUpdaterRepository.UpdateAboutUsSection(id, aboutUsRequestDTO);

            if (result is null)
            {
                _logger.LogWarning("No se encontró AboutUsSection para actualizar con Id: {Id}", id);
                throw new NotFoundException($"No se encontró AboutUsSection para actualizar con Id: {id}");
            }

            _logger.LogInformation("Actualización exitosa de AboutUsSection con Id: {Id}", id);

            _memoryCachesService.RemoveById(Constants.CACHE_ABOUT_US, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_ABOUT_US);

            return result.ToAboutUsResponseDTO();
            
        }
    }
}

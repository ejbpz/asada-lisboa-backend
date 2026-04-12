using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsUpdaterService : IConfigurationsUpdaterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ILogger<ConfigurationsUpdaterService> _logger;
        private readonly IConfigurationsUpdaterRepository _configurationsUpdaterRepository;

        public ConfigurationsUpdaterService(IConfigurationsUpdaterRepository configurationsUpdaterRepository, ILogger<ConfigurationsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _configurationsUpdaterRepository = configurationsUpdaterRepository;
        }

        public async Task<ConfigurationResponseDTO> UpdateConfiguration(Guid id, ConfigurationsRequestDTO configurationRequestDTO)
        {
           var result = await _configurationsUpdaterRepository.UpdateConfiguration(id, configurationRequestDTO);

            if (result is null){
                _logger.LogWarning("No se encontró configuración para actualizar con Id: {Id}", id);
                throw new NotFoundException($"No se encontró configuración para actualizar con Id: {id}");
            }

            _logger.LogInformation("Actualización exitosa de configuración con Id: {Id}", id);

            _memoryCachesService.RemoveById(Constants.CACHE_CONFIGURATIONS, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_CONFIGURATIONS);

            return  result.ToConfigurationResponseDTO();
        }
    }
}

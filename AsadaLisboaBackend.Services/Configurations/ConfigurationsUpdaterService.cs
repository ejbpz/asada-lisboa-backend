using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Services.MemoryCaches;
using AsadaLisboaBackend.Utils;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsUpdaterService : IConfigurationsUpdaterService
    {
        private readonly IConfigurationsUpdaterRepository _configurationsUpdaterRepository;
        private readonly ILogger<ConfigurationsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ConfigurationsUpdaterService(IConfigurationsUpdaterRepository configurationsUpdaterRepository, ILogger<ConfigurationsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _configurationsUpdaterRepository = configurationsUpdaterRepository;            _logger = logger;
            _memoryCachesService = memoryCachesService;

        }

        public async Task<ConfigurationResponseDTO> UpdateConfiguration(Guid id, ConfigurationsRequestDTO configurationRequestDTO)
        {
           var result = await _configurationsUpdaterRepository.UpdateConfiguration(id, configurationRequestDTO);

            if (result = null){
                _logger.LogWarning("No se encontró configuración para actualizar con Id: {Id}", id);
            }

            _logger.LogInformation("Actualización exitosa de configuración con Id: {Id}", id);

            _memoryCachesService.RemoveById(Constants.CACHE_CONFIGURATIONS, id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_CONFIGURATIONS);

            return  result.ToConfigurationResponseDTO();
        }
    }
}

using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsDeleterService : IConfigurationsDeleterService
    {
        private readonly IConfigurationsDeleterRepository _configurationsDeleterRepository;
        private readonly ILogger<ConfigurationsDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ConfigurationsDeleterService(IConfigurationsDeleterRepository configurationsDeleterRepository, ILogger<ConfigurationsDeleterService> logger, IMemoryCachesService memoryCachesService)
        {
            _configurationsDeleterRepository = configurationsDeleterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task UpdateConfiguration(Guid id)
        {
            try 
            {
              await _configurationsDeleterRepository.DeleteConfiguration(id);

                _memoryCachesService.RemoveById(Constants.CACHE_CONFIGURATIONS, id);
                _memoryCachesService.ChangeVersion(Constants.CACHE_CONFIGURATIONS);

                _logger.LogInformation("Configuración con Id: {Id} eliminado exitosamente.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminar  configuración con Id: {Id}", id);
                throw;
            }

        }
    }
}

using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsDeleterService : IConfigurationsDeleterService
    {
        private readonly IConfigurationsDeleterRepository _configurationsDeleterRepository;
        private readonly ILogger<ConfigurationsDeleterService> _logger;

        public ConfigurationsDeleterService(IConfigurationsDeleterRepository configurationsDeleterRepository, ILogger<ConfigurationsDeleterService> logger)
        {
            _configurationsDeleterRepository = configurationsDeleterRepository;
            _logger = logger;
        }

        public async Task UpdateConfiguration(Guid id)
        {
            try 
            {
              await _configurationsDeleterRepository.DeleteConfiguration(id);
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

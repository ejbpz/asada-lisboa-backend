using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsGetterService : IConfigurationsGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ILogger<ConfigurationsGetterService> _logger;
        private readonly IConfigurationsGetterRepository _configurationsGetterRepository;

        public ConfigurationsGetterService(IConfigurationsGetterRepository configurationsGetterRepository, ILogger<ConfigurationsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _configurationsGetterRepository = configurationsGetterRepository;
        }

        public async Task<PageResponseDTO<ConfigurationResponseDTO>> GetConfigurations(SearchSortRequestDTO searchSortRequestDTO)
        {
            try
            {
                var result = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<ConfigurationResponseDTO>>(

                    resource: Constants.CACHE_CONFIGURATIONS,
                    request: searchSortRequestDTO,
                    create: () => _configurationsGetterRepository.GetConfigurations(searchSortRequestDTO),
                    time: TimeSpan.FromMinutes(5));

                _logger.LogInformation(
                    "Obtención exitosa de configuración. Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener configuración. Tamaño: {Take}",
                    searchSortRequestDTO.Take
                );
                throw;
            }
        }
    }
}

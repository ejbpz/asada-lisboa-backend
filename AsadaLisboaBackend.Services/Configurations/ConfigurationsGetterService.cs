using AsadaLisboaBackend.Models.DTOs.AboutUs;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.RepositoryContracts.AboutUsSections;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsGetterService : IConfigurationsGetterService
    {
        private readonly IConfigurationsGetterRepository _configurationsGetterRepository;
        private readonly ILogger<ConfigurationsGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;

        public ConfigurationsGetterService(IConfigurationsGetterRepository configurationsGetterRepository, ILogger<ConfigurationsGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _configurationsGetterRepository = configurationsGetterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<PageResponseDTO<ConfigurationResponseDTO>> GetConfigurations(SearchSortRequestDTO searchSortRequestDTO)
        {
            try
            {
                searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

                var result  = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<AboutUsResponseDTO>>(

                    resource: Constants.CACHE_CONFIGURATIONS,
                    request: searchSortRequestDTO,
                    create: () => _configurationsGetterRepository.GetConfigurations(searchSortRequestDTO),
                    time: TimeSpan.FromMinutes(5));

                _logger.LogInformation(
                    "Obtención exitosa de configuración. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener configuración. Página: {Page}, Tamaño: {Take}",
                    searchSortRequestDTO.Page,
                    searchSortRequestDTO.Take
                );
                throw;
            }
    }
}

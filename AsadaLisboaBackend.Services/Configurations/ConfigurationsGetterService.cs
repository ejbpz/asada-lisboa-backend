using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Configuration;
using AsadaLisboaBackend.ServiceContracts.Configurations;
using AsadaLisboaBackend.RepositoryContracts.Configurations;
using Microsoft.Extensions.Logging;

namespace AsadaLisboaBackend.Services.Configurations
{
    public class ConfigurationsGetterService : IConfigurationsGetterService
    {
        private readonly IConfigurationsGetterRepository _configurationsGetterRepository;
        private readonly ILogger<ConfigurationsGetterService> _logger;

        public ConfigurationsGetterService(IConfigurationsGetterRepository configurationsGetterRepository, ILogger<ConfigurationsGetterService> logger)
        {
            _configurationsGetterRepository = configurationsGetterRepository;
            _logger = logger;
        }

        public async Task<PageResponseDTO<ConfigurationResponseDTO>> GetConfigurations(SearchSortRequestDTO searchSortRequestDTO)
        {
            try
            {
                searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

                var result  = await _configurationsGetterRepository.GetConfigurations(searchSortRequestDTO);

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

using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.Services.Statuses
{
    public class StatusesGetterService : IStatusesGetterService
    {
        private readonly IStatusesGetterRepository _statusesGetterRepository;
        private readonly ILogger<StatusesGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        public StatusesGetterService(IStatusesGetterRepository statusesGetterRepository, ILogger<StatusesGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _statusesGetterRepository = statusesGetterRepository;
            _logger = logger;
            _memoryCachesService = memoryCachesService;
        }

        public async Task<List<StatusResponseDTO>> GetStatuses()
        {
            try
            {
                var result = await _memoryCachesService.GetOrCreateCache<List<StatusResponseDTO>>(
                key: Constants.CACHE_CATEGORIES,
                create: () => _statusesGetterRepository.GetStatuses(),
                time: TimeSpan.FromMinutes(5));

                _logger.LogInformation("Obtención exitosa de estados.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los estados");
                throw;
            }
        }
    }
}

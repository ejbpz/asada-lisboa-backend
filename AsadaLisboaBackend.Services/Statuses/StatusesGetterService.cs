using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Statuses;

public class StatusesGetterService(
    IStatusesGetterRepository statusesGetterRepository,
    ILogger<StatusesGetterService> logger,
    IMemoryCachesService memoryCachesService)
    : IStatusesGetterService
{
    public async Task<List<StatusResponseDto>> GetStatuses()
    {
        try
        {
            var result = await memoryCachesService.GetOrCreateCache(
                key: Constants.CACHE_CATEGORIES,
                create: statusesGetterRepository.GetStatuses,
                time: TimeSpan.FromMinutes(5));

            logger.LogInformation("Obtención exitosa de estados.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al obtener los estados");
            throw;
        }
    }
}
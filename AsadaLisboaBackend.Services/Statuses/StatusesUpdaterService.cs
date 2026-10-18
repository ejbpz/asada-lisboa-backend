using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Statuses;

public class StatusesUpdaterService(
    IStatusesGetterRepository statusesGetterRepository,
    IStatusesUpdaterRepository statusesUpdaterRepository,
    ILogger<StatusesUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IStatusesUpdaterService
{
    public async Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType)
    {
        try
        {
            var status = await statusesGetterRepository.GetStatus(statusId);
            await statusesUpdaterRepository.ChangeStatus(objectId, status.Id, objectType);

            logger.LogInformation(
                "Cambio de estado exitoso. ObjectId: {ObjectId}, NuevoStatusId: {StatusId}, Tipo: {ObjectType}",
                objectId,
                status.Id,
                objectType
            );

            memoryCachesService.RemoveById(Constants.CACHE_STATUSES, objectId);
            memoryCachesService.ChangeVersion(Constants.CACHE_STATUSES);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error al cambiar el estado. ObjectId: {ObjectId}, StatusId: {StatusId}, Tipo: {ObjectType}",
                objectId,
                statusId,
                objectType
            );
            throw;
        }
    }
}
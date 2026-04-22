using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Statuses
{
    public class StatusesUpdaterService : IStatusesUpdaterService
    {
        private readonly ILogger<StatusesUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;
        private readonly IStatusesUpdaterRepository _statusesUpdaterRepository;

        public StatusesUpdaterService(IStatusesGetterRepository statusesGetterRepository, IStatusesUpdaterRepository statusesUpdaterRepository, ILogger<StatusesUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _statusesGetterRepository = statusesGetterRepository;
            _statusesUpdaterRepository = statusesUpdaterRepository;
        }

        public async Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType)
        {
            try
            {

                var status = await _statusesGetterRepository.GetStatus(statusId);

                if (status == null)
                {
                    _logger.LogWarning(
                        "No se encontró el estado con StatusId: {StatusId} para ObjectId: {ObjectId}",
                        statusId,
                        objectId
                    );
                    throw new InvalidOperationException("El estado no existe");
                }

                await _statusesUpdaterRepository.ChangeStatus(objectId, status.Id, objectType);

                _logger.LogInformation(
                    "Cambio de estado exitoso. ObjectId: {ObjectId}, NuevoStatusId: {StatusId}, Tipo: {ObjectType}",
                    objectId,
                    status.Id,
                    objectType
                );

                _memoryCachesService.RemoveById(Constants.CACHE_STATUSES, objectId);
                _memoryCachesService.ChangeVersion(Constants.CACHE_STATUSES);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al cambiar el estado. ObjectId: {ObjectId}, StatusId: {StatusId}, Tipo: {ObjectType}",
                    objectId,
                    statusId,
                    objectType
                );
                throw;
            }
        }
    }
}

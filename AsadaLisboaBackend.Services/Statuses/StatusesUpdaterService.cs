using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Services.Statuses
{
    public class StatusesUpdaterService : IStatusesUpdaterService
    {
        private readonly IStatusesGetterRepository _statusesGetterRepository;
        private readonly IStatusesUpdaterRepository _statusesUpdaterRepository;

        public StatusesUpdaterService(IStatusesGetterRepository statusesGetterRepository, IStatusesUpdaterRepository statusesUpdaterRepository)
        {
            _statusesGetterRepository = statusesGetterRepository;
            _statusesUpdaterRepository = statusesUpdaterRepository;
        }

        public async Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType)
        {
            var status = await _statusesGetterRepository.GetStatus(statusId);

            await _statusesUpdaterRepository.ChangeStatus(objectId, status.Id, objectType);
        }
    }
}

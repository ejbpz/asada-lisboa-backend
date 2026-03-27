using AsadaLisboaBackend.Models.Enums;

namespace AsadaLisboaBackend.ServiceContracts.Statuses
{
    public interface IStatusesUpdaterService
    {
        public Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType);
    }
}

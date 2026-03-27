using AsadaLisboaBackend.Models.Enums;

namespace AsadaLisboaBackend.RepositoryContracts.Statuses
{
    public interface IStatusesUpdaterRepository
    {
        public Task ChangeStatus(Guid objectId, Guid statusId, ObjectTypeEnum objectType);
    }
}

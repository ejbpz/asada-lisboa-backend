using AsadaLisboaBackend.Models.DTOs.Status;

namespace AsadaLisboaBackend.ServiceContracts.Statuses
{
    public interface IStatusesGetterService
    {
        public Task<List<StatusResponseDTO>> GetStatuses();
    }
}

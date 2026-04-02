using AsadaLisboaBackend.Models.DTOs.Status;

namespace AsadaLisboaBackend.RepositoryContracts.Statuses
{
    public interface IStatusesGetterRepository
    {
        public Task<List<StatusResponseDTO>> GetStatuses();
        public Task<StatusResponseDTO> GetStatus(Guid id);
        public Task<Guid> GetStatusPublicado();
    }
}

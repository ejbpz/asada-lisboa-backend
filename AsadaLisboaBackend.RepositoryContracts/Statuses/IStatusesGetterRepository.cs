using AsadaLisboaBackend.Models.DTOs.Status;

namespace AsadaLisboaBackend.RepositoryContracts.Statuses;

public interface IStatusesGetterRepository
{
    public Task<List<StatusResponseDto>> GetStatuses();
    public Task<StatusResponseDto> GetStatus(Guid id);
}
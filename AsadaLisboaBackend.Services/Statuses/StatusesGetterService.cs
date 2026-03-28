using AsadaLisboaBackend.Models.DTOs.Status;
using AsadaLisboaBackend.ServiceContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Statuses;

namespace AsadaLisboaBackend.Services.Statuses
{
    public class StatusesGetterService : IStatusesGetterService
    {
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public StatusesGetterService(IStatusesGetterRepository statusesGetterRepository)
        {
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<List<StatusResponseDTO>> GetStatuses()
        {
            return await _statusesGetterRepository.GetStatuses();
        }
    }
}

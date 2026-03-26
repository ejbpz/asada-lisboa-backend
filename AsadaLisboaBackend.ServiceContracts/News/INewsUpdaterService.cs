using AsadaLisboaBackend.Models.DTOs.New;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsUpdaterService
    {
        public Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO);
    }
}

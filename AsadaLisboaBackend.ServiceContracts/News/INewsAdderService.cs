using AsadaLisboaBackend.Models.DTOs.New;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsAdderService
    {
        public Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO);
    }
}

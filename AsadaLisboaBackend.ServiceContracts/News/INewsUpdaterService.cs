using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsUpdaterService
    {
        public Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO, FileStorageOptions options);
    }
}

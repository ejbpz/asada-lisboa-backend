using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsAdderService
    {
        public Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO, FileStorageOptions fileStorageOptions);
    }
}

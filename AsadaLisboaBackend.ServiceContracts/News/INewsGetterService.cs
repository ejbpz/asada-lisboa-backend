using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.News
{
    public interface INewsGetterService
    {
        public Task<PageResponseDTO<NewResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO);
        public Task<NewResponseDTO> GetNew(Guid id);
    }
}

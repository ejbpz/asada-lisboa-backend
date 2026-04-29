using AsadaLisboaBackend.Models.DTOs.SearchGlobal;

namespace AsadaLisboaBackend.ServiceContracts.SearchGlobal
{
    public interface ISearchGlobalService
    {
        public Task<List<SearchGlobalResponseDTO>> Search(SearchGlobalRequestDTO request);
    }
}

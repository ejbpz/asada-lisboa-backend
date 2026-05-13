using AsadaLisboaBackend.Models.DTOs.Search;

namespace AsadaLisboaBackend.ServiceContracts.Searches
{
    public interface ISearchesGetterService
    {
        public Task<List<SearchResponseDTO>> Search(string query);
    }
}

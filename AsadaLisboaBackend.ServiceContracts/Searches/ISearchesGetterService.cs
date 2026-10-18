using AsadaLisboaBackend.Models.DTOs.Search;

namespace AsadaLisboaBackend.ServiceContracts.Searches;

public interface ISearchesGetterService
{
    public Task<List<SearchResponseDto>> Search(string query);
}
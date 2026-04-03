using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.News
{
    public interface INewsGetterRepository
    {
        public Task<PageResponseDTO<NewMinimalResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO);
        public Task<NewResponseDTO> GetNewBySlug(string slug);
        public Task<NewResponseDTO> GetNew(Guid id);
    }
}

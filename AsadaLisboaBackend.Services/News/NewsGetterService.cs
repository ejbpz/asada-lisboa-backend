using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsGetterService : INewsGetterService
    {
        private readonly INewsGetterRepository _newsGetterRepository;

        public NewsGetterService(INewsGetterRepository newsGetterRepository)
        {
            _newsGetterRepository = newsGetterRepository;
        }

        public async Task<NewResponseDTO> GetNew(Guid id)
        {
            return await _newsGetterRepository.GetNew(id);
        }

        public async Task<PageResponseDTO<NewResponseDTO>> GetNews(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _newsGetterRepository.GetNews(searchSortRequestDTO);
        }
    }
}

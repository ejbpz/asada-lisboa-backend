using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.News;

public interface INewsGetterService
{
    public Task<PageResponseDto<NewMinimalResponseDto>> GetNews(SearchSortRequestDto searchSortRequestDto);
    public Task<NewResponseDto> GetNewBySlug(string slug);
    public Task<NewResponseDto> GetNew(Guid id);
    public Task<List<NewMinimalResponseDto>> GetRecommendedNews(string slug);
}
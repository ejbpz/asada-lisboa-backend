using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.AboutUs;

namespace AsadaLisboaBackend.RepositoryContracts.AboutUsSections;

public interface IAboutUsSectionsGetterRepository
{
    public Task<PageResponseDto<AboutUsResponseDto>> GetAboutUsSections(SearchSortRequestDto searchSortRequestDto);
}
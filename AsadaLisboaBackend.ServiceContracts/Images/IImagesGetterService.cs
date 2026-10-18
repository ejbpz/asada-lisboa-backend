using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.Images;

public interface IImagesGetterService
{
    public Task<PageResponseDto<ImageMinimalResponseDto>> GetImages(SearchSortRequestDto searchSortRequestDto); 
    public Task<ImageResponseDto> GetImageBySlug(string slug);
    public Task<ImageResponseDto> GetImage(Guid id);
}
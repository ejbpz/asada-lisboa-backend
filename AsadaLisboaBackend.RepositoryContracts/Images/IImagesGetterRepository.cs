using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.Images;

public interface IImagesGetterRepository
{
    public Task<PageResponseDto<ImageMinimalResponseDto>> GetImages(SearchSortRequestDto searchSortRequestDto);
    public Task<Image> GetImageBySlug(string slug);
    public Task<Image> GetImage(Guid id);
}
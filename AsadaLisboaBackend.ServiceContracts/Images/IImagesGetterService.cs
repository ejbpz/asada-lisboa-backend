using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.Images
{
    public interface IImagesGetterService
    {
        public Task<PageResponseDTO<ImageMinimalResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO); 
        public Task<ImageResponseDTO> GetImageBySlug(string slug);
        public Task<ImageResponseDTO> GetImage(Guid id);
    }
}

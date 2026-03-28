using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.Image
{
    public interface IImagesGetterService
    {
        public Task<PageResponseDTO<ImageResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO);
        public Task<ImageResponseDTO> GetImage(Guid id);
    }
}

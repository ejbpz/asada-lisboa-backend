using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.Images
{
    public interface IImagesGetterRepository
    {
        public Task<PageResponseDTO<ImageResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO);
        public Task<Image> GetImage(Guid id);
    }
}

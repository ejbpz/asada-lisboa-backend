using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImagesGetterService : IImagesGetterService
    {
        private readonly IImagesGetterRepository _imagesGetterRepository;

        public ImagesGetterService(IImagesGetterRepository imagesGetterRepository)
        {
            _imagesGetterRepository = imagesGetterRepository;
        }

        public async Task<PageResponseDTO<ImageResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _imagesGetterRepository.GetImages(searchSortRequestDTO);
        }

        public async Task<ImageResponseDTO> GetImage(Guid id)
        {
            return await _imagesGetterRepository.GetImage(id);
        }
    }
}

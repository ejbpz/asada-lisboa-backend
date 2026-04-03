using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesGetterService : IImagesGetterService
    {
        private readonly IImagesGetterRepository _imagesGetterRepository;

        public ImagesGetterService(IImagesGetterRepository imagesGetterRepository)
        {
            _imagesGetterRepository = imagesGetterRepository;
        }

        public async Task<PageResponseDTO<ImageMinimalResponseDTO>> GetImages(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _imagesGetterRepository.GetImages(searchSortRequestDTO);
        }

        public async Task<ImageResponseDTO> GetImage(Guid id)
        {
            return (await _imagesGetterRepository.GetImage(id))
                .ToImageResponseDTO();
        }

        public async Task<ImageResponseDTO> GetImageBySlug(string slug)
        {
            return (await _imagesGetterRepository.GetImageBySlug(slug))
                .ToImageResponseDTO();
        }
    }
}

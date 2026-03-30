using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.ServiceContracts.Images
{
    public interface IImagesAdderService
    {
        public Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO);
    }
}

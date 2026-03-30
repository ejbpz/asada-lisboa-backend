using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.ServiceContracts.Image
{
    public interface IImagesAdderService
    {
        public Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO);
    }
}

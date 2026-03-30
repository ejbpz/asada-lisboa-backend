using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.ServiceContracts.Images
{
    public interface IImagesUpdaterService
    {
        public Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO);

    }
}

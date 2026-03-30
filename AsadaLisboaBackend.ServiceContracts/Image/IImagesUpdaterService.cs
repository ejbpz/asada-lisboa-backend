using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.ServiceContracts.Image
{
    public interface IImagesUpdaterService
    {
        public Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO);

    }
}

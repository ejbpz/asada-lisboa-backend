using AsadaLisboaBackend.Models.DTOs.Image;

namespace AsadaLisboaBackend.ServiceContracts.Images;

public interface IImagesUpdaterService
{
    public Task<ImageResponseDto> UpdateImage(Guid id, ImageUpdateRequestDto imageUpdateRequestDto);

}
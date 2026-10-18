using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images;

public class ImagesDeleterService(
    IFileSystemsManager fileSystems,
    IImagesDeleterRepository imagesDeleterRepository,
    IImagesGetterRepository imagesGetterRepository,
    ILogger<ImagesDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : IImagesDeleterService
{
    public async Task DeleteImage(Guid id)
    {
        var image = await imagesGetterRepository.GetImage(id);

        if (!string.IsNullOrEmpty(image.FileName) && !string.IsNullOrWhiteSpace(image.FileName))
            await fileSystems.DeleteAsync(image.FileName, "imagenes");

        await imagesDeleterRepository.DeleteImage(id);

        memoryCachesService.RemoveById(Constants.CACHE_IMAGES, image.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

        logger.LogInformation("Imagen con id {Id} eliminada correctamente.", id);
    }
}
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images;

public class ImagesUpdaterService(
    IFileSystemsManager fileSystems, 
    IImagesUpdaterRepository imagesUpdaterRepository, 
    IImagesGetterRepository imagesGetterRespository, 
    ICategoriesGetterService categoriesGetterService, 
    IStatusesGetterRepository statusesGetterRepository, 
    ILogger<ImagesUpdaterService> logger, 
    IMemoryCachesService memoryCachesService) 
    : IImagesUpdaterService
{
    public async Task<ImageResponseDto> UpdateImage(Guid id, ImageUpdateRequestDto imageUpdateRequestDto)
    {
        var image = await imagesGetterRespository.GetImage(id);

        image.Title = imageUpdateRequestDto.Title;

        if (imageUpdateRequestDto.StatusId == Guid.Empty)
        {
            logger.LogError("StatusId no puede ser vacío.");
            throw new ArgumentException("StatusId inválido.");
        }

        var status = await statusesGetterRepository.GetStatus(imageUpdateRequestDto.StatusId);

        image.StatusId = status.Id;
        image.Description = imageUpdateRequestDto.Description;
        image.Slug = GenerateSlug.New(imageUpdateRequestDto.Title, image.Id);
        image.Categories = await categoriesGetterService.ToCreateCategories(imageUpdateRequestDto.Categories);

        if (imageUpdateRequestDto.File is not null && imageUpdateRequestDto.File.Length > 0)
        {
            var newUrl = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(image.FilePath) && File.Exists(image.FilePath))
                    File.Delete(image.FilePath);

                newUrl = await fileSystems.SaveAsync(imageUpdateRequestDto.File, "imagenes", image.Slug);
                var newFileName = Path.GetFileName(newUrl);
                    
                image.Url = newUrl;
                image.FileName = newFileName;
                image.FilePath = $"imagenes/{newFileName}";
                image.FileSize = imageUpdateRequestDto.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl) && !string.IsNullOrWhiteSpace(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await fileSystems.DeleteAsync(fileName, "imagenes");
                }

                logger.LogError("Error al actualizar la imagen con id {DocumentId}.", id);
                throw new CreateObjectException("Error al actualizar la imagen.");
            }
        }

        var imageUpdated = await imagesUpdaterRepository.UpdateImage(image);
        logger.LogInformation("Imagen con id {DocumentId} actualizada correctamente.", imageUpdated.Id);

        memoryCachesService.RemoveById(Constants.CACHE_IMAGES, imageUpdated.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

        return imageUpdated.ToImageResponseDto();
    }
}
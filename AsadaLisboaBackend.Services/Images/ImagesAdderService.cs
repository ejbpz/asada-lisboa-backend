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

public class ImagesAdderService(
    IImagesAdderRepository imagesAdderRepository,
    IFileSystemsManager fileSystems,
    ICategoriesGetterService categoriesGetterService,
    IStatusesGetterRepository statusesGetterRepository,
    ILogger<ImagesAdderService> logger,
    IMemoryCachesService memoryCachesService)
    : IImagesAdderService
{
    public async Task<ImageResponseDto> CreateImage(ImageRequestDto imageRequestDto)
    {
        if (imageRequestDto.File is null || imageRequestDto.File.Length == 0)
        {
            logger.LogError("Archivo inválido. No se proporcionó un archivo o el archivo está vacío.");
            throw new ArgumentException("Archivo inválido.");
        }

        var imageId = Guid.NewGuid();

        var url = string.Empty;

        try
        {
            var slug = GenerateSlug.New(imageRequestDto.Title, imageId);
            url = await fileSystems.SaveAsync(imageRequestDto.File, "imagenes", slug);

            var fileName = Path.GetFileName(url);
            var filePath = $"imagenes/{fileName}";

            if (imageRequestDto.StatusId == Guid.Empty)
            {
                logger.LogError("StatusId no puede ser vacío.");
                throw new ArgumentException("StatusId inválido.");
            }

            var status = await statusesGetterRepository.GetStatus(imageRequestDto.StatusId);
            var categories = await categoriesGetterService.ToCreateCategories(imageRequestDto.Categories);

            var image = new Models.Image()
            {
                Id = imageId,
                Url = url,
                Slug = slug,
                FilePath = filePath,
                FileName = fileName,
                StatusId = status.Id,
                Categories = categories,
                Title = imageRequestDto.Title,
                PublicationDate = DateTime.UtcNow,
                FileSize = imageRequestDto.File.Length,
                Description = imageRequestDto.Description,
            };

            var imageCreated = await imagesAdderRepository.CreateImage(image);
            logger.LogInformation("Imagen creada exitosamente con id: {ImageId}", imageCreated.Id);

            memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

            return imageCreated.ToImageResponseDto();
        }
        catch
        {
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
            {
                var fileName = Path.GetFileName(url);
                await fileSystems.DeleteAsync(fileName, "imagenes");
            }

            logger.LogError("Error al crear la imagen. Se eliminó la imagen subida con éxito.");
            throw new CreateObjectException("Error al crear la imagen.");
        }
    }
}
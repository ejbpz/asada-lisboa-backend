using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesUpdaterService : IImagesUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<ImagesUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IImagesGetterRepository _imagesGetterRespository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IImagesUpdaterRepository _imagesUpdaterRepository;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public ImagesUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemsManager fileSystems, IImagesUpdaterRepository imagesUpdaterRepository, IImagesGetterRepository imagesGetterRespository, ICategoriesGetterService categoriesGetterService, IStatusesGetterRepository statusesGetterRepository, ILogger<ImagesUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _categoriesGetterService = categoriesGetterService;
            _imagesUpdaterRepository = imagesUpdaterRepository;
            _imagesGetterRespository = imagesGetterRespository;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO)
        {
            var image = await _imagesGetterRespository.GetImage(id);

            if (image is null)
            {
                _logger.LogError("Imagen con {Id}, no encontrada.", id);
                throw new NotFoundException("Imagen no encontrada.");
            }

            image.Title = imageUpdateRequestDTO.Title;

            if (imageUpdateRequestDTO.StatusId == Guid.Empty)
            {
                _logger.LogError("StatusId no puede ser vacío.");
                throw new ArgumentException("StatusId inválido.");
            }

            var status = await _statusesGetterRepository.GetStatus(imageUpdateRequestDTO.StatusId);

            if (status is null) 
            {
                _logger.LogError("Status con {StatusId}, no encontrado.", imageUpdateRequestDTO.StatusId);
                throw new NotFoundException("Status no encontrado.");
            }

            image.StatusId = status.Id;
            image.Description = imageUpdateRequestDTO.Description;

            image.Slug = GenerateSlug.New(imageUpdateRequestDTO.Title, image.Id);

            image.Categories = await _categoriesGetterService.ToCreateCategories(imageUpdateRequestDTO.Categories);

            if (imageUpdateRequestDTO.File is not null && imageUpdateRequestDTO.File.Length > 0)
            {
                string? newUrl = string.Empty;

                try
                {
                    if (!string.IsNullOrEmpty(image.FilePath) && File.Exists(image.FilePath))
                        File.Delete(image.FilePath);

                    newUrl = await _fileSystems.SaveAsync(imageUpdateRequestDTO.File, "imagenes", image.Slug);
                    var newFileName = Path.GetFileName(newUrl);
                    
                    image.Url = newUrl;
                    image.FileName = newFileName;
                    image.FilePath = $"imagenes/{newFileName}";
                    image.FileSize = imageUpdateRequestDTO.File.Length;
                }
                catch
                {
                    if (!string.IsNullOrEmpty(newUrl) && !string.IsNullOrWhiteSpace(newUrl))
                    {
                        var fileName = Path.GetFileName(newUrl);
                        await _fileSystems.DeleteAsync(fileName, "imagenes");
                    }

                    _logger.LogError("Error al actualizar la imagen con id {DocumentId}.", id);
                    throw new CreateObjectException("Error al actualizar la imagen.");
                }
            }

            var imageUpdated = await _imagesUpdaterRepository.UpdateImage(image);
            _logger.LogInformation("Imagen con id {DocumentId} actualizada correctamente.", imageUpdated.Id);

            _memoryCachesService.RemoveById(Constants.CACHE_IMAGES, imageUpdated.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

            return imageUpdated.ToImageResponseDTO();
        }
    }
}
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

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesAdderService : IImagesAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<ImagesAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IImagesAdderRepository _imagesAdderRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public ImagesAdderService(IImagesAdderRepository imagesAdderRepository, IFileSystemsManager fileSystems, ICategoriesGetterService categoriesGetterService, IStatusesGetterRepository statusesGetterRepository, ILogger<ImagesAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _imagesAdderRepository = imagesAdderRepository;
            _categoriesGetterService = categoriesGetterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO)
        {
            if (imageRequestDTO.File is null || imageRequestDTO.File.Length == 0)
            {
                _logger.LogError("Archivo inválido. No se proporcionó un archivo o el archivo está vacío.");
                throw new ArgumentException("Archivo inválido.");
            }

            var imageId = Guid.NewGuid();

            string? url = string.Empty;

            try
            {
                var slug = GenerateSlug.New(imageRequestDTO.Title, imageId);
                url = await _fileSystems.SaveAsync(imageRequestDTO.File, "imagenes", slug);

                var fileName = Path.GetFileName(url);
                var filePath = $"imagenes/{fileName}";

                if (imageRequestDTO.StatusId == Guid.Empty)
                {
                    _logger.LogError("StatusId no puede ser vacío.");
                    throw new ArgumentException("StatusId inválido.");
                }

                var status = await _statusesGetterRepository.GetStatus(imageRequestDTO.StatusId);
                
                if (status is null)
                {
                    _logger.LogError("Status no encontrado con id: {Id}.", imageRequestDTO.StatusId);
                    throw new NotFoundException("Status no encontrado.");
                }

                var categories = await _categoriesGetterService.ToCreateCategories(imageRequestDTO.Categories);

                var image = new Models.Image()
                {
                    Id = imageId,
                    Url = url,
                    Slug = slug,
                    FilePath = filePath,
                    FileName = fileName,
                    StatusId = status.Id,
                    Categories = categories,
                    Title = imageRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
                    FileSize = imageRequestDTO.File.Length,
                    Description = imageRequestDTO.Description,
                };

                var imageCreated = await _imagesAdderRepository.CreateImage(image);
                _logger.LogInformation("Imagen creada exitosamente con id: {ImageId}", imageCreated.Id);

                _memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

                return imageCreated.ToImageResponseDTO();
            }
            catch
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
                {
                    var fileName = Path.GetFileName(url);
                    await _fileSystems.DeleteAsync(fileName, "imagenes");
                }

                _logger.LogError("Error al crear la imagen. Se eliminó la imagen subida con éxito.");
                throw new CreateObjectException("Error al crear la imagen.");
            }
        }
    }
}

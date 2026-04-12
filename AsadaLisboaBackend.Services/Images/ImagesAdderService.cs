using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
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
        private readonly ElasticsearchClient _elastic;
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<ImagesAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IImagesAdderRepository _imagesAdderRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public ImagesAdderService(IImagesAdderRepository imagesAdderRepository, IFileSystemsManager fileSystems, ICategoriesGetterService categoriesGetterService, IStatusesGetterRepository statusesGetterRepository, ILogger<ImagesAdderService> logger, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
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
                url = await _fileSystems.SaveAsync(imageRequestDTO.File, "images");

                var fileName = Path.GetFileName(url);
                var filePath = $"images/{fileName}";

                var slug = GenerateSlug.New(imageRequestDTO.Title, imageId);

                var status = await _statusesGetterRepository.GetStatus(imageRequestDTO.StatusId);

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

                //Add to ElasticSearch
                var imag = new Models.DTOs.SearchGlobal.SearchGlobalResponseDTO
                {
                    Id = image.Id,
                    Type = "Imagen",
                    Title = image.Title,
                    Description = image.Description,
                    Slug = image.Slug,
                };
                await _elastic.IndexAsync(imag);

                return imageCreated.ToImageResponseDTO();
            }
            catch
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
                {
                    var fileName = Path.GetFileName(url);
                    await _fileSystems.DeleteAsync(fileName, "images");
                }

                _logger.LogError("Error al crear la imagen. Se eliminó la imagen subida con éxito.");
                throw new CreateObjectException("Error al crear la imagen.");
            }
        }
    }
}

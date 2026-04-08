using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using Elastic.Clients.Elasticsearch;

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
        private readonly ElasticsearchClient _elastic;

        public ImagesUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemsManager fileSystems, IImagesUpdaterRepository imagesUpdaterRepository, IImagesGetterRepository imagesGetterRespository, ICategoriesGetterService categoriesGetterService, ILogger<ImagesUpdaterService> logger, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _categoriesGetterService = categoriesGetterService;
            _imagesUpdaterRepository = imagesUpdaterRepository;
            _imagesGetterRespository = imagesGetterRespository;
            _elastic = elastic;
        }

        public async Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO)
        {
            var image = await _imagesGetterRespository.GetImage(id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            image.Title = imageUpdateRequestDTO.Title;
            image.StatusId = imageUpdateRequestDTO.StatusId;
            image.Description = imageUpdateRequestDTO.Description;

            image.Slug = GenerateSlug.New(imageUpdateRequestDTO.Title, image.Id);

            image.Categories = await _categoriesGetterService.ToCreateCategories(imageUpdateRequestDTO.Categories);

            if (imageUpdateRequestDTO.File is null || imageUpdateRequestDTO.File.Length <= 0)
                throw new ArgumentNullException("Error al actualizar la imagen.");

            string? newUrl = string.Empty;

            try
            {
                newUrl = await _fileSystems.SaveAsync(imageUpdateRequestDTO.File, "images");

                var newFileName = Path.GetFileName(newUrl);

                if (!string.IsNullOrEmpty(image.FilePath) && !string.IsNullOrWhiteSpace(image.FilePath) && File.Exists(image.FilePath) && image.FilePath != newUrl)
                    File.Delete(image.FilePath);

                image.Url = newUrl;
                image.FileName = newFileName;
                image.FilePath = $"images/{newFileName}";
                image.FileSize = imageUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl) && !string.IsNullOrWhiteSpace(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await _fileSystems.DeleteAsync(fileName, "images");
                }

                _logger.LogError("Error al actualizar la imagen con id {DocumentId}.", id);
                throw new CreateObjectException("Error al actualizar la imagen.");
            }

            var imageUpdated = await _imagesUpdaterRepository.UpdateImage(image);
            _logger.LogInformation("Imagen con id {DocumentId} actualizada correctamente.", imageUpdated.Id);

            _memoryCachesService.RemoveById(Constants.CACHE_IMAGES, imageUpdated.Id);
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

            return imageUpdated.ToImageResponseDTO();
        }
    }
}
using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesDeleterService : IImagesDeleterService
    {
        private readonly ElasticsearchClient _elastic;
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<ImagesDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IImagesGetterRepository _imagesGetterRepository;
        private readonly IImagesDeleterRepository _imagesDeleterRepository;

        public ImagesDeleterService(IFileSystemsManager fileSystems, IImagesDeleterRepository imagesDeleterRepository, IImagesGetterRepository imagesGetterRepository, ILogger<ImagesDeleterService> logger,IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _imagesGetterRepository = imagesGetterRepository;
            _imagesDeleterRepository = imagesDeleterRepository;
        }

        public async Task DeleteImage(Guid id)
        {
            var image = await _imagesGetterRepository.GetImage(id);

            if (image is null)
            {
                _logger.LogError("Imagen con id {Id} no encontrada.", id);
                throw new NotFoundException("Imagen no encontrada.");
            }

            if (!string.IsNullOrEmpty(image.FileName) && !string.IsNullOrWhiteSpace(image.FileName))
                await _fileSystems.DeleteAsync(image.FileName, "images");

            await _imagesDeleterRepository.DeleteImage(id);

            // Eliminar del índice
            await _elastic.DeleteAsync<Image>(id, d => d.Index("imagenes"));

            _memoryCachesService.RemoveById(Constants.CACHE_IMAGES, image.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_IMAGES);

            _logger.LogInformation("Imagen con id {Id} eliminada correctamente.", id);
        }
    }
}

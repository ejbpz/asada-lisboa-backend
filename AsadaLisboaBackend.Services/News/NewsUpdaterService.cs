using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.HtmlSanitizer;
using AsadaLisboaBackend.Utils.SlugGeneration;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsUpdaterService : INewsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<NewsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly INewsGetterRepository _newsGetterRepository;
        private readonly IEditorsUpdaterService _editorsUpdaterService;
        private readonly IEditorsDeleterService _editorsDeleterService;
        private readonly INewsUpdaterRepository _newsUpdaterRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsUpdaterService(INewsUpdaterRepository newsUpdaterRepository, INewsGetterRepository newsGetterRepository, IEditorsUpdaterService editorsUpdaterService, IEditorsDeleterService editorsDeleterService, IStatusesGetterRepository statusesGetterRepository, ICategoriesGetterService categoriesGetterService, IFileSystemsManager fileSystems, ILogger<NewsUpdaterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _newsGetterRepository = newsGetterRepository;
            _editorsDeleterService = editorsDeleterService;
            _editorsUpdaterService = editorsUpdaterService;
            _newsUpdaterRepository = newsUpdaterRepository;
            _categoriesGetterService = categoriesGetterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<NewResponseDTO> UpdateNew(Guid id, NewRequestDTO newRequestDTO)
        {
            var existingNew = await _newsGetterRepository.GetNew(id);

            if (existingNew is null)
            {
                _logger.LogError("Noticia con id {Id} no encontrada.", id);
                throw new NotFoundException("Noticia no encontrada.");
            }

            var imageUrl = existingNew.ImageUrl;
            var fileName = existingNew.FileName;
            var filePath = existingNew.FilePath;

            existingNew.Slug = GenerateSlug.New(newRequestDTO.Title, id);

            if (newRequestDTO.File is not null)
            {
                if (!string.IsNullOrEmpty(existingNew.FilePath) && File.Exists(existingNew.FilePath))
                    File.Delete(existingNew.FilePath);

                var newImageUrl = await _fileSystems.SaveAsync(newRequestDTO.File, "noticias", existingNew.Slug);

                imageUrl = newImageUrl;
                fileName = Path.GetFileName(imageUrl);
                filePath = $"noticias/{fileName}";
            }

            var sanitizer = HtmlSanitizerFactory.Create();
            var cleanHtml = sanitizer.Sanitize(newRequestDTO.Description);

            var content = await _editorsUpdaterService.ChangeHtmlImagesFolder(cleanHtml);
            await _editorsDeleterService.DeleteUnusedImages(existingNew.Description, newRequestDTO.Description);

            if (newRequestDTO.StatusId == Guid.Empty)
            {
                _logger.LogError("StatusId no puede ser vacío.");
                throw new ArgumentException("StatusId inválido.");
            }

            var status = await _statusesGetterRepository.GetStatus(newRequestDTO.StatusId);

            if (status is null)
            {
                _logger.LogError("Status con {StatusId}, no encontrado.", newRequestDTO.StatusId);
                throw new NotFoundException("Status no encontrado.");
            }

            var categories = await _categoriesGetterService.ToCreateCategories(newRequestDTO.Categories);


            var newModel = new New()
            {
                Id = id,
                StatusId = status.Id,
                ImageUrl = imageUrl,
                FileName = fileName,
                FilePath = filePath,
                Description = content,
                Categories = categories,
                Slug = existingNew.Slug,
                Title = newRequestDTO.Title,
                LastEditionDate = DateTime.UtcNow,
                PublicationDate = existingNew.PublicationDate,
            };

            var updated = await _newsUpdaterRepository.UpdateNew(id, newModel);

            if (updated is null)
            {
                _logger.LogError("Error al actualizar la noticia con id {Id}", id);
                throw new UpdateObjectException("Error al actualizar la noticia.");
            }

            _logger.LogInformation("Noticia con id {Id} actualizada correctamente", updated.Id);

            _memoryCachesService.RemoveById(Constants.CACHE_NEWS, updated.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

            return updated.ToNewResponseDTO();
        }
    }
}

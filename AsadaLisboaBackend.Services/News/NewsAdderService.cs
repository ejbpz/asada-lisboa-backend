using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.HtmlSanitizer;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.News
{
    public class NewsAdderService : INewsAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<NewsAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly INewsAdderRepository _newsAdderRepository;
        private readonly IEditorsUpdaterService _editorsUpdaterService;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public NewsAdderService(INewsAdderRepository newsAdderRepository, IEditorsUpdaterService editorsUpdaterService, IStatusesGetterRepository statusesGetterRepository, ICategoriesGetterService categoriesGetterService, IFileSystemsManager fileSystems, ILogger<NewsAdderService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _newsAdderRepository = newsAdderRepository;
            _editorsUpdaterService = editorsUpdaterService;
            _categoriesGetterService = categoriesGetterService;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<NewResponseDTO> CreateNew(NewRequestDTO newRequestDTO)
        {
            var id = Guid.NewGuid();

            string imageUrl = string.Empty;
            string fileName = string.Empty;
            string filePath = string.Empty;

            string slug = GenerateSlug.New(newRequestDTO.Title, id);

            if (newRequestDTO.File is not null && newRequestDTO.File.Length > 0)
            {
                imageUrl = await _fileSystems.SaveAsync(newRequestDTO.File, "noticias", slug);
                fileName = Path.GetFileName(imageUrl);
            }

            if(!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrWhiteSpace(imageUrl) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrWhiteSpace(fileName))
                filePath = $"noticias/{fileName}";

            var sanitizer = HtmlSanitizerFactory.Create();
            var cleanHtml = sanitizer.Sanitize(newRequestDTO.Description);

            var content = await _editorsUpdaterService.ChangeHtmlImagesFolder(cleanHtml);

            if (newRequestDTO.StatusId == Guid.Empty)
            {
                _logger.LogError("StatusId no puede ser vacío.");
                throw new ArgumentException("StatusId inválido.");
            }

            var status = await _statusesGetterRepository.GetStatus(newRequestDTO.StatusId);

            if (status is null)
            {
                _logger.LogError("Status no encontrado con id: {Id}.", newRequestDTO.StatusId);
                throw new NotFoundException("Status no encontrado.");
            }

            var categories = await _categoriesGetterService.ToCreateCategories(newRequestDTO.Categories);

            var newModel = new New()
            {
                Id = id,
                StatusId = status.Id,
                Slug = slug,
                FilePath = filePath,
                FileName = fileName,
                ImageUrl = imageUrl,
                Description = content,
                Categories = categories,
                Title = newRequestDTO.Title,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
            };

            var created = await _newsAdderRepository.CreateNew(newModel);

            if (created is null)
            {
                _logger.LogError("Error al crear la noticia con id {Id}.", id);
                throw new CreateObjectException("Error al crear la noticia.");
            }

            _logger.LogInformation("Noticia con id {Id} creada exitosamente.", created.Id);

            _memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

            return created.ToNewResponseDTO();
        }
    }
}

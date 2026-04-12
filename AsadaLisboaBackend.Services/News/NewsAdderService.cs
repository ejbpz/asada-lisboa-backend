using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.New;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.News;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.ServiceContracts.Editors;
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
        private readonly ElasticsearchClient _elastic;

        public NewsAdderService(INewsAdderRepository newsAdderRepository, IEditorsUpdaterService editorsUpdaterService, IStatusesGetterRepository statusesGetterRepository, ICategoriesGetterService categoriesGetterService, IFileSystemsManager fileSystems, ILogger<NewsAdderService> logger, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
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

            if (newRequestDTO.File is not null && newRequestDTO.File.Length > 0)
            {
                imageUrl = await _fileSystems.SaveAsync(newRequestDTO.File, "news");
                fileName = Path.GetFileName(imageUrl);
            }

            if(!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrWhiteSpace(imageUrl) && !string.IsNullOrEmpty(fileName) && !string.IsNullOrWhiteSpace(fileName))
                filePath = $"news/{fileName}";

            var content = await _editorsUpdaterService.ChangeHtmlImagesFolder(newRequestDTO.Description);

            var categories = await _categoriesGetterService.ToCreateCategories(newRequestDTO.Categories);

            var status = await _statusesGetterRepository.GetStatus(newRequestDTO.StatusId);

            var newModel = new New()
            {
                Id = id,
                StatusId = status.Id,
                ImageUrl = imageUrl,
                FileName = fileName,
                FilePath = filePath,
                Description = content,
                Categories = categories,
                Title = newRequestDTO.Title,
                PublicationDate = DateTime.UtcNow,
                LastEditionDate = DateTime.UtcNow,
                Slug = GenerateSlug.New(newRequestDTO.Title, id),
            };

            var created = await _newsAdderRepository.CreateNew(newModel);

            if (created is null)
            {
                _logger.LogError("Error al crear la noticia con id {Id}.", id);
                throw new CreateObjectException("Error al crear la noticia.");
            }

            _logger.LogInformation("Noticia con id {Id} creada exitosamente.", created.Id);

            _memoryCachesService.ChangeVersion(Constants.CACHE_NEWS);

            //Add to ElasticSearch
            var news = new Models.DTOs.SearchGlobal.SearchGlobalResponseDTO
            {
                Id = newModel.Id,
                Type = "Noticias",
                Title = newModel.Title,
                Description = newModel.Description,
                Slug = newModel.Slug,
            };
            await _elastic.IndexAsync(news);

            return created.ToNewResponseDTO();
        }
    }
}

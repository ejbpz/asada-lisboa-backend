using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;
using Elastic.Clients.Elasticsearch;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsAdderService: IDocumentsAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<DocumentsAdderService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IDocumentsAdderRepository _documentAdderRepository;
        private readonly IStatusesGetterRepository _statusesGetterRepository;
        private readonly IDocumentTypesGetterRepository _documentTypesGetterRepository;
        private readonly ElasticsearchClient _elastic;

        public DocumentsAdderService(ICategoriesGetterService categoriesGetterService, IDocumentsAdderRepository documentAdderRepository, IStatusesGetterRepository statusesGetterRepository, IDocumentTypesGetterRepository documentTypesGetterRepository, IFileSystemsManager fileSystems, ILogger<DocumentsAdderService> logger, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _categoriesGetterService = categoriesGetterService;
            _documentAdderRepository = documentAdderRepository;
            _statusesGetterRepository = statusesGetterRepository;
            _documentTypesGetterRepository = documentTypesGetterRepository;
            _elastic = elastic;
        }

        public async Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO)
        {
            if (documentRequestDTO.File is null || documentRequestDTO.File.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var documentId = Guid.NewGuid();

            string? url = string.Empty;

            try
            {
                url = await _fileSystems.SaveAsync(documentRequestDTO.File, "documents");

                var fileName = Path.GetFileName(url);
                var filePath = $"documents/{fileName}";

                var slug = GenerateSlug.New(documentRequestDTO.Title, documentId);

                var status = await _statusesGetterRepository.GetStatus(documentRequestDTO.StatusId);

                var categories = await _categoriesGetterService.ToCreateCategories(documentRequestDTO.Categories);

                var extension = Path.GetExtension(url);
                var documentTypeId = _documentTypesGetterRepository.GetDocumentTypeIdByExtension(extension);

                if (documentTypeId is null || !documentTypeId.HasValue)
                    throw new NotFoundException("Tipo de documento no soportado.");

                var document = new Models.Document()
                {
                    Id = documentId,
                    Url = url,
                    Slug = slug,
                    FileName = fileName,
                    FilePath = filePath,
                    StatusId = status.Id,
                    Categories = categories,
                    Title = documentRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
                    DocumentTypeId = documentTypeId.Value,
                    FileSize = documentRequestDTO.File.Length,
                    Description = documentRequestDTO.Description,
                };

                var documentCreated = await _documentAdderRepository.CreateDocument(document);
                _logger.LogInformation("Documento creado exitosamente con id: {DocumentId}", documentCreated.Id);

                _memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

                //Add to ElasticSearch
                var doc = new Models.DTOs.SearchGlobal.SearchGlobalResponseDTO                   
                {
                    Id = document.Id,
                    Type = "Documento",
                    Title = document.Title,
                    Description = document.Description,
                    
                };
                await _elastic.IndexAsync(doc);

                return documentCreated.ToDocumentResponseDTO();
            }
            catch
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
                {
                    var fileName = Path.GetFileName(url);
                    await _fileSystems.DeleteAsync(fileName, "documents");
                }

                _logger.LogError("Error al crear el documento. Se eliminó el archivo subido con éxito.");
                throw new CreateObjectException("Error al crear el documento.");
            }
        }
    }
}

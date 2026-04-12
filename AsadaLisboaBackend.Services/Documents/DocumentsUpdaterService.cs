using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsUpdaterService : IDocumentsUpdaterService
    {
        private readonly ElasticsearchClient _elastic;
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<DocumentsUpdaterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IDocumentsGetterRepository _documentGetterRepository;
        private readonly IDocumentsUpdaterRepository _documentUpdateRespository;
        private readonly IDocumentTypesGetterRepository _documentTypesGetterRepository;

        public DocumentsUpdaterService(IFileSystemsManager fileSystems, IDocumentsGetterRepository documentGetterRepository, IDocumentsUpdaterRepository documentUpdateRespository, ICategoriesGetterService categoriesGetterService, IDocumentTypesGetterRepository documentTypesGetterRepository, ILogger<DocumentsUpdaterService> logger, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _categoriesGetterService = categoriesGetterService;
            _documentGetterRepository = documentGetterRepository;
            _documentUpdateRespository = documentUpdateRespository;
            _documentTypesGetterRepository = documentTypesGetterRepository;
        }

        public async Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            var document = await _documentGetterRepository.GetDocument(id);

            if (document is null)
            {
                _logger.LogError("Documento con id {DocumentId} no encontrado para actualizar.", id);
                throw new NotFoundException("Documento no encontrado.");
            }

            document.Title = documentUpdateRequestDTO.Title;
            document.StatusId = documentUpdateRequestDTO.StatusId;
            document.Description = documentUpdateRequestDTO.Description;

            document.Slug = GenerateSlug.New(documentUpdateRequestDTO.Title, document.Id);

            document.Categories = await _categoriesGetterService.ToCreateCategories(documentUpdateRequestDTO.Categories);

            if (documentUpdateRequestDTO.File is null || documentUpdateRequestDTO.File.Length <= 0)
            {
                _logger.LogError("Archivo no proporcionado para actualizar el documento con id {DocumentId}.", id);
                throw new NotFoundException("Error al actualizar el documento.");
            }

            string? newUrl = string.Empty;

            try
            {
                newUrl = await _fileSystems.SaveAsync(documentUpdateRequestDTO.File, "documents");

                var newFileName = Path.GetFileName(newUrl);

                if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath) && document.FilePath != newUrl)
                    File.Delete(document.FilePath);

                document.Url = newUrl;
                document.FileName = newFileName;
                document.FilePath = $"documents/{newFileName}";
                document.FileSize = documentUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl) && string.IsNullOrWhiteSpace(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await _fileSystems.DeleteAsync(fileName, "documents");
                }

                _logger.LogError("Error al actualizar el documento con id {DocumentId}.", id);
                throw new CreateObjectException("Error al actualizar el documento.");
            }

            var extension = Path.GetExtension(newUrl);
            var documentTypeId = _documentTypesGetterRepository.GetDocumentTypeIdByExtension(extension);

            if (documentTypeId is null || !documentTypeId.HasValue)
                throw new NotFoundException("Tipo de documento no soportado.");

            document.DocumentTypeId = documentTypeId.Value;

            var documentUpdated = await _documentUpdateRespository.UpdateDocument(document);

            _memoryCachesService.RemoveById(Constants.CACHE_DOCUMENTS, documentUpdated.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

            _logger.LogInformation("Documento con id {DocumentId} actualizado correctamente.", documentUpdated.Id);

            var doc = new Models.DTOs.SearchGlobal.SearchGlobalResponseDTO
            {
                Id = document.Id,
                Type = "Documento",
                Title = document.Title,
                Description = document.Description,

            };
            await _elastic.IndexAsync(doc);

            return documentUpdated.ToDocumentResponseDTO();
        }
    }
}
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

namespace AsadaLisboaBackend.Services.Documents;

public class DocumentsAdderService(
    ICategoriesGetterService categoriesGetterService,
    IDocumentsAdderRepository documentAdderRepository,
    IStatusesGetterRepository statusesGetterRepository,
    IDocumentTypesGetterRepository documentTypesGetterRepository,
    IFileSystemsManager fileSystems,
    ILogger<DocumentsAdderService> logger,
    IMemoryCachesService memoryCachesService)
    : IDocumentsAdderService
{
    public async Task<DocumentResponseDto> CreateDocument(DocumentRequestDto documentRequestDto)
    {
        if (documentRequestDto.File is null || documentRequestDto.File.Length == 0)
        {
            logger.LogError("Archivo nulo o inválido a ser agregado.");
            throw new ArgumentException("Archivo inválido.");
        }

        var documentId = Guid.NewGuid();

        var url = string.Empty;

        try
        {
            var slug = GenerateSlug.New(documentRequestDto.Title, documentId);
            url = await fileSystems.SaveAsync(documentRequestDto.File, "documentos", slug);

            var fileName = Path.GetFileName(url);
            var filePath = $"documentos/{fileName}";

            if(documentRequestDto.StatusId == Guid.Empty)
            {
                logger.LogError("StatusId no puede ser vacío.");
                throw new ArgumentException("StatusId inválido.");
            }

            var status = await statusesGetterRepository.GetStatus(documentRequestDto.StatusId);

            var categories = await categoriesGetterService.ToCreateCategories(documentRequestDto.Categories);

            var extension = Path.GetExtension(url);
            var documentTypeId = documentTypesGetterRepository.GetDocumentTypeIdByExtension(extension);

            if (documentTypeId is null || documentTypeId == Guid.Empty)
            {
                logger.LogError("Tipo de documento no encontrado con id: {Id}.", documentTypeId);
                throw new NotFoundException("Tipo de documento no soportado.");
            }

            var document = new Models.Document()
            {
                Id = documentId,
                Url = url,
                Slug = slug,
                FileName = fileName,
                FilePath = filePath,
                StatusId = status.Id,
                Categories = categories,
                Title = documentRequestDto.Title,
                PublicationDate = DateTime.UtcNow,
                DocumentTypeId = documentTypeId.Value,
                FileSize = documentRequestDto.File.Length,
                Description = documentRequestDto.Description,
            };

            var documentCreated = await documentAdderRepository.CreateDocument(document);
            logger.LogInformation("Documento creado exitosamente con id: {DocumentId}", documentCreated.Id);

            memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

            return documentCreated.ToDocumentResponseDto();
        }
        catch
        {
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
            {
                var fileName = Path.GetFileName(url);
                await fileSystems.DeleteAsync(fileName, "documentos");
            }

            logger.LogError("Error al crear el documento. Se eliminó el archivo subido con éxito.");
            throw new CreateObjectException("Error al crear el documento.");
        }
    }
}
using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;

namespace AsadaLisboaBackend.Services.Documents;

public class DocumentsUpdaterService(
    IFileSystemsManager fileSystems,
    IDocumentsGetterRepository documentGetterRepository,
    IDocumentsUpdaterRepository documentUpdateRespository,
    ICategoriesGetterService categoriesGetterService,
    IDocumentTypesGetterRepository documentTypesGetterRepository,
    IStatusesGetterRepository statusesGetterRepository,
    ILogger<DocumentsUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IDocumentsUpdaterService
{
    public async Task<DocumentResponseDto> UpdateDocument(Guid id, DocumentUpdateRequestDto documentUpdateRequestDto)
    {
        var document = await documentGetterRepository.GetDocument(id);
        document.Title = documentUpdateRequestDto.Title;

        if (documentUpdateRequestDto.StatusId == Guid.Empty)
        {
            logger.LogError("StatusId no puede ser vacío.");
            throw new ArgumentException("StatusId inválido.");
        }

        var status = await statusesGetterRepository.GetStatus(documentUpdateRequestDto.StatusId);

        document.StatusId = status.Id;
        document.Description = documentUpdateRequestDto.Description;
        document.Slug = GenerateSlug.New(documentUpdateRequestDto.Title, document.Id);
        document.Categories = await categoriesGetterService.ToCreateCategories(documentUpdateRequestDto.Categories);

        if (documentUpdateRequestDto.File is not null && documentUpdateRequestDto.File.Length > 0)
        {
            var newUrl = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))
                    File.Delete(document.FilePath);
                    
                newUrl = await fileSystems.SaveAsync(documentUpdateRequestDto.File, "documentos", document.Slug);
                var newFileName = Path.GetFileName(newUrl);

                document.Url = newUrl;
                document.FileName = newFileName;
                document.FilePath = $"documentos/{newFileName}";
                document.FileSize = documentUpdateRequestDto.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl) && string.IsNullOrWhiteSpace(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await fileSystems.DeleteAsync(fileName, "documentos");
                }

                logger.LogError("Error al actualizar el documento con id {DocumentId}.", id);
                throw new CreateObjectException("Error al actualizar el documento.");
            }

            var extension = Path.GetExtension(newUrl);
            var documentTypeId = documentTypesGetterRepository.GetDocumentTypeIdByExtension(extension);

            if (documentTypeId is null || !documentTypeId.HasValue)
                throw new NotFoundException("Tipo de documento no soportado.");

            document.DocumentTypeId = documentTypeId.Value;
        }

        var documentUpdated = await documentUpdateRespository.UpdateDocument(document);

        memoryCachesService.RemoveById(Constants.CACHE_DOCUMENTS, documentUpdated.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

        logger.LogInformation("Documento con id {DocumentId} actualizado correctamente.", documentUpdated.Id);

        return documentUpdated.ToDocumentResponseDto();
    }
}
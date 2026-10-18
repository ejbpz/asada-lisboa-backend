using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents;

public class DocumentsDeleterService(
    IFileSystemsManager fileSystems,
    ILogger<DocumentsDeleterService> logger,
    IDocumentsDeleterRepository documentsDeleterRespository,
    IDocumentsGetterRepository documentsGetterRespository,
    IMemoryCachesService memoryCachesService)
    : IDocumentsDeleterService
{
    public async Task DeleterDocument(Guid id)
    {
        var document = await documentsGetterRespository.GetDocument(id);

        if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))
            await fileSystems.DeleteAsync(document.FileName, "documentos");

        await documentsDeleterRespository.DeleteDocument(id);

        memoryCachesService.RemoveById(Constants.CACHE_DOCUMENTS, document.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

        logger.LogInformation("Documento con id {DocumentId} eliminado correctamente.", id);
    }
}
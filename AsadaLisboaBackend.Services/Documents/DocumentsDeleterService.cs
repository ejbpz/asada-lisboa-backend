using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsDeleterService: IDocumentsDeleterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly IDocumentsDeleterRepository _documentsDeleterRespository;

      public DocumentsDeleterService(IFileSystemsManager fileSystems, IDocumentsDeleterRepository documentsDeleterRespository)
        {
            _fileSystems = fileSystems;
            _documentsDeleterRespository = documentsDeleterRespository;
        }

        public async Task DeleterDocument(Guid id)
        {
            var document = await _documentsDeleterRespository.DeleteDocument(id);

            if (document is null)
                throw new NotFoundException("Documento no encontrado.");

            if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))

                await _fileSystems.DeleteAsync(document.FileName, "document");

            await _documentsDeleterRespository.DeleteDocument(id);
        }
    }
}

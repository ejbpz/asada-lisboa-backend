using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Document;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Services.Document
{
    public class DocumentDeleterService: IDocumentDeleterService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFileSystemManager _fileSystem;
        private readonly IDocumentDeleterRespository _documentDeleterRespository;

      public DocumentDeleterService(ApplicationDbContext applicationDbContext, IFileSystemManager fileSystem, IDocumentDeleterRespository documentDeleterRespository)
        {
            _applicationDbContext = applicationDbContext;
            _documentDeleterRespository = documentDeleterRespository;
            _fileSystem = fileSystem;
        }

        public async Task DeleterDocument(Guid id)
        {
            var document = await _documentDeleterRespository.DeleteDocument(id);

            if (document is null)
                throw new NotFoundException("Documento no encontrado.");

            if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))

                await _fileSystem.DeleteAsync(document.FileName, "document");

            await _documentDeleterRespository.DeleteDocument(id);

        }
    }
}

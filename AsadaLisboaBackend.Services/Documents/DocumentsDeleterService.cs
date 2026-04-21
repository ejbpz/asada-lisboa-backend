using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsDeleterService: IDocumentsDeleterService
    {
        private readonly ElasticsearchClient _elastic;
        private readonly IFileSystemsManager _fileSystems;
        private readonly ILogger<DocumentsDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IDocumentsGetterRepository _documentsGetterRespository;
        private readonly IDocumentsDeleterRepository _documentsDeleterRespository;

        public DocumentsDeleterService(IFileSystemsManager fileSystems, ILogger<DocumentsDeleterService> logger, IDocumentsDeleterRepository documentsDeleterRespository, IDocumentsGetterRepository documentsGetterRespository, IMemoryCachesService memoryCachesService, ElasticsearchClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
            _fileSystems = fileSystems;
            _memoryCachesService = memoryCachesService;
            _documentsGetterRespository = documentsGetterRespository;
            _documentsDeleterRespository = documentsDeleterRespository;
        }

        public async Task DeleterDocument(Guid id)
        {
            var document = await _documentsGetterRespository.GetDocument(id);

            if (document is null)
            {
                _logger.LogError("Documento con id: {Id} no encontrado.", id);
                throw new NotFoundException("Documento no encontrado.");
            }

            if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))
                await _fileSystems.DeleteAsync(document.FileName, "documentos");

            await _documentsDeleterRespository.DeleteDocument(id);

            await _elastic.DeleteAsync<Document>(id, d => d.Index("documentos"));


            _memoryCachesService.RemoveById(Constants.CACHE_DOCUMENTS, document.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_DOCUMENTS);

            _logger.LogInformation("Documento con id {DocumentId} eliminado correctamente.", id);
        }
    }
}

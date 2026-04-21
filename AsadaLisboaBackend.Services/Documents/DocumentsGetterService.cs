using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsGetterService : IDocumentsGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IDocumentsGetterRepository _documentGetterRepository;

        public DocumentsGetterService(IDocumentsGetterRepository documentGetterRepository, IMemoryCachesService memoryCachesService)
        {
            _memoryCachesService = memoryCachesService;
            _documentGetterRepository = documentGetterRepository;
        }

        public async Task<PageResponseDTO<DocumentMinimalResponseDTO>> GetDocuments(SearchSortRequestDTO searchSortRequestDTO)
        {
            return await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<DocumentMinimalResponseDTO>>(
                resource: Constants.CACHE_DOCUMENTS,
                request: searchSortRequestDTO, 
                create: () => _documentGetterRepository.GetDocuments(searchSortRequestDTO),
                time: TimeSpan.FromMinutes(5));
        }

        public async Task<DocumentResponseDTO> GetDocument(Guid id) 
        {
            return (await _memoryCachesService.GetOrCreateCache<Document>(
                key: $"{Constants.CACHE_DOCUMENTS}_{id}",
                create: () => _documentGetterRepository.GetDocument(id),
                time: TimeSpan.FromMinutes(5)))
                    .ToDocumentResponseDTO();
        }

        public async Task<DocumentResponseDTO> GetDocumentBySlug(string slug)
        {
            return (await _memoryCachesService.GetOrCreateCache<Document>(
                key: $"{Constants.CACHE_DOCUMENTS}_{slug}",
                create: () => _documentGetterRepository.GetDocumentBySlug(slug),
                time: TimeSpan.FromMinutes(5)))
                    .ToDocumentResponseDTO();
        }
    }
}

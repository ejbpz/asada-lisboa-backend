using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Documents;

public class DocumentsGetterService(
    IDocumentsGetterRepository documentGetterRepository,
    IMemoryCachesService memoryCachesService)
    : IDocumentsGetterService
{
    public async Task<PageResponseDto<DocumentMinimalResponseDto>> GetDocuments(SearchSortRequestDto searchSortRequestDto)
    {
        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_DOCUMENTS,
            request: searchSortRequestDto, 
            create: () => documentGetterRepository.GetDocuments(searchSortRequestDto),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<DocumentResponseDto> GetDocument(Guid id) 
    {
        return (await memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_DOCUMENTS}_{id}",
                create: () => documentGetterRepository.GetDocument(id),
                time: TimeSpan.FromMinutes(5)))
            .ToDocumentResponseDto();
    }

    public async Task<DocumentResponseDto> GetDocumentBySlug(string slug)
    {
        return (await memoryCachesService.GetOrCreateCache(
                key: $"{Constants.CACHE_DOCUMENTS}_{slug}",
                create: () => documentGetterRepository.GetDocumentBySlug(slug),
                time: TimeSpan.FromMinutes(5)))
            .ToDocumentResponseDto();
    }
}
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents;

public interface IDocumentsGetterService
{
    public Task<PageResponseDto<DocumentMinimalResponseDto>> GetDocuments(SearchSortRequestDto searchSortRequestDto);
    public Task<DocumentResponseDto> GetDocumentBySlug(string slug);
    public Task<DocumentResponseDto> GetDocument(Guid id);
}
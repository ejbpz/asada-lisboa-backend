using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents;

public interface IDocumentsUpdaterService
{
    public Task<DocumentResponseDto> UpdateDocument(Guid id, DocumentUpdateRequestDto documentUpdateRequestDto); 
}
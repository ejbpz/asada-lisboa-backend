using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents;

public interface IDocumentsAdderService
{
    public Task<DocumentResponseDto> CreateDocument(DocumentRequestDto documentRequestDto);
}
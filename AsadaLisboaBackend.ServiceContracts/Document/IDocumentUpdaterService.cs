using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Document
{
    public interface IDocumentUpdaterService
    {
        public Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO); 
    }
}

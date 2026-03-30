using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents
{
    public interface IDocumentsUpdaterService
    {
        public Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO); 
    }
}

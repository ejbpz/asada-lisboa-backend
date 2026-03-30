using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents
{
    public interface IDocumentsAdderService
    {
        public Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO);
    }
}

using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Document
{
    public interface IDocumentAdderService
    {
        public Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO);
    }
}

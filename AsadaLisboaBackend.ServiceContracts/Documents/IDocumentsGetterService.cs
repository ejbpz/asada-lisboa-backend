using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents
{
    public interface IDocumentsGetterService
    {
        public Task<PageResponseDTO<DocumentMinimalResponseDTO>> GetDocuments(SearchSortRequestDTO searchSortRequestDTO);
        public Task<DocumentResponseDTO> GetDocument(Guid id);
    }
}

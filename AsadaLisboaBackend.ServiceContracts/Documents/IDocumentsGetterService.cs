using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Documents
{
    public interface IDocumentsGetterService
    {
        public Task<PageResponseDTO<DocumentResponseDTO>> GetDocument(SearchSortRequestDTO searchSortRequestDTO);
        public Task<DocumentResponseDTO> GetDocument(Guid id);
    }
}

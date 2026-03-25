using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.ServiceContracts.Document
{
    public interface IDocumentGetterService
    {
        public Task<PageResponseDTO<DocumentResponseDTO>> GetDocument(SearchSortRequestDTO searchSortRequestDTO);
        public Task<DocumentResponseDTO> GetDocument(Guid id);
    }
}

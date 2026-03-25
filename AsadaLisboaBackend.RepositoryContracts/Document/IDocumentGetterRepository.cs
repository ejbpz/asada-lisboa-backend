using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.Document
{
    public interface IDocumentGetterRepository
    {
        public Task<PageResponseDTO<DocumentResponseDTO>> GetDocument(SearchSortRequestDTO searchSortRequestDTO);

        public Task<DocumentResponseDTO> GetDocument(Guid id);
    }
}

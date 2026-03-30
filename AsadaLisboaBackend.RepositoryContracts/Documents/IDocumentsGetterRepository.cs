using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;

namespace AsadaLisboaBackend.RepositoryContracts.Documents
{
    public interface IDocumentsGetterRepository
    {
        public Task<PageResponseDTO<DocumentResponseDTO>> GetDocument(SearchSortRequestDTO searchSortRequestDTO);

        public Task<DocumentResponseDTO> GetDocument(Guid id);
    }
}

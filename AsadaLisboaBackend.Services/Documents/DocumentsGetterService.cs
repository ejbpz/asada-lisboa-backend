using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsGetterService : IDocumentsGetterService
    {
        private readonly IDocumentsGetterRepository _documentGetterRepository;

        public DocumentsGetterService(IDocumentsGetterRepository documentGetterRepository)
        {
            _documentGetterRepository = documentGetterRepository;
        }

        public async Task<PageResponseDTO<DocumentResponseDTO>> GetDocument(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _documentGetterRepository.GetDocument(searchSortRequestDTO);

        }

        public async Task<DocumentResponseDTO> GetDocument(Guid id) 
        {
            return await _documentGetterRepository.GetDocument(id);

        }
    }
}

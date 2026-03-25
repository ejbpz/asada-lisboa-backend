using AsadaLisboaBackend.ServiceContracts.Document;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.RepositoryContracts.Document;

namespace AsadaLisboaBackend.Services.Document
{
    public class DocumentGetterService : IDocumentGetterService
    {
        private readonly IDocumentGetterRepository _documentGetterRepository;

        public DocumentGetterService(IDocumentGetterRepository documentGetterRepository)
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

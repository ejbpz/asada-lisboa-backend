using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.Document
{
   public interface IDocumentService
    {
        public Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO, FileStorageOptions fileStorageOptions);
    }
}

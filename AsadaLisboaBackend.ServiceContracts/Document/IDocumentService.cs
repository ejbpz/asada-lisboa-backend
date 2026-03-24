using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServiceContracts.Document
{
   public interface IDocumentService
    {
        public Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO, FileStorageOptions fileStorageOptions);

        public Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO, FileStorageOptions fileStorageOptions);

        public Task<bool> DeleteImage(Guid id);
    }
}

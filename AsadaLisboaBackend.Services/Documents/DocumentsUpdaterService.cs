using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsUpdaterService : IDocumentsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IDocumentsGetterRepository _documentGetterRepository;
        private readonly IDocumentsUpdaterRepository _documentUpdateRespository;

        public DocumentsUpdaterService(IFileSystemsManager fileSystems, IDocumentsGetterRepository documentGetterRepository, IDocumentsUpdaterRepository documentUpdateRespository, ICategoriesGetterService categoriesGetterService)
        {
            _fileSystems = fileSystems;
            _categoriesGetterService = categoriesGetterService;
            _documentGetterRepository = documentGetterRepository;
            _documentUpdateRespository = documentUpdateRespository;
        }

        public async Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO)
        {
            var document = await _documentGetterRepository.GetDocument(id);

            if (document is null)
                throw new NotFoundException("Documento no encontrado.");

            document.Title = documentUpdateRequestDTO.Title;
            document.StatusId = documentUpdateRequestDTO.StatusId;
            document.Description = documentUpdateRequestDTO.Description;

            document.Slug = GenerateSlug.New(documentUpdateRequestDTO.Title, document.Id);

            document.Categories = await _categoriesGetterService.ToCreateCategories(documentUpdateRequestDTO.Categories);

            if (documentUpdateRequestDTO.File is null || documentUpdateRequestDTO.File.Length <= 0)
                throw new NotFoundException("Error al actualizar el documento.");

            string? newUrl = string.Empty;

            try
            {
                newUrl = await _fileSystems.SaveAsync(documentUpdateRequestDTO.File, "documents");

                var newFileName = Path.GetFileName(newUrl);

                if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath) && document.FilePath != newUrl)
                    File.Delete(document.FilePath);

                document.Url = newUrl;
                document.FileName = newFileName;
                document.FilePath = $"documents/{newFileName}";
                document.FileSize = documentUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await _fileSystems.DeleteAsync(fileName, "documents");
                }

                throw new CreateObjectException("Error al actualizar el documento.");
            }

            return (await _documentUpdateRespository.UpdateDocument(document))
                .ToDocumentResponseDTO();
        }
    }
}
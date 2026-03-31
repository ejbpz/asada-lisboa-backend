using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsAdderService: IDocumentsAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IDocumentsAdderRepository _documentAdderRepository;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public DocumentsAdderService(ICategoriesGetterService categoriesGetterService, IDocumentsAdderRepository documentAdderRepository, IStatusesGetterRepository statusesGetterRepository, IFileSystemsManager fileSystems)
        {
            _fileSystems = fileSystems;
            _categoriesGetterService = categoriesGetterService;
            _documentAdderRepository = documentAdderRepository;
            _statusesGetterRepository = statusesGetterRepository;
        }

        public async Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO)
        {
            if (documentRequestDTO.File is null || documentRequestDTO.File.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var documentId = Guid.NewGuid();

            string? url = string.Empty;

            try
            {
                url = await _fileSystems.SaveAsync(documentRequestDTO.File, "documents");

                var fileName = Path.GetFileName(url);
                var filePath = $"documents/{fileName}";

                var slug = GenerateSlug.New(documentRequestDTO.Title, documentId);

                var status = await _statusesGetterRepository.GetStatus(documentRequestDTO.StatusId);

                var categories = await _categoriesGetterService.ToCreateCategories(documentRequestDTO.Categories);

                var document = new Models.Document()
                {
                    Id = documentId,
                    Url = url,
                    Slug = slug,
                    FileName = fileName,
                    FilePath = filePath,
                    StatusId = status.Id,
                    Categories = categories,
                    Title = documentRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
                    FileSize = documentRequestDTO.File.Length,
                    Description = documentRequestDTO.Description,
                    DocumentTypeId = documentRequestDTO.DocumentTypeId,
                };

                return (await _documentAdderRepository.CreateDocument(document))
                    .ToDocumentResponseDTO();
            }
            catch
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var fileName = Path.GetFileName(url);
                    await _fileSystems.DeleteAsync(fileName, "documents");
                }

                throw new CreateObjectException("Error al crear el documento.");
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.FileSystems;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Services.Documents
{
    public class DocumentsAdderService: IDocumentsAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IDocumentsAdderRepository _documentAdderRepository;

        public DocumentsAdderService(ApplicationDbContext applicationDbContext, IDocumentsAdderRepository documentAdderRepository, IFileSystemsManager fileSystems)
        {
            _fileSystems = fileSystems;
            _applicationDbContext = applicationDbContext;
            _documentAdderRepository = documentAdderRepository;
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

                var status = await _applicationDbContext.Statuses
                    .FirstOrDefaultAsync(c => c.Id == documentRequestDTO.StatusId);

                //var categories = await _applicationDbContext.Categories
                //    .Where(c => documentRequestDTO.CategoryIds.Contains(c.Id))
                //    .ToListAsync();

                var document = new Models.Document()
                {
                    Id = documentId,
                    Url = url,
                    Slug = slug,
                    Status = status,
                    FileName = fileName,
                    FilePath = filePath,
                    //Categories = categories,
                    Title = documentRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
                    StatusId = documentRequestDTO.StatusId,
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

using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DTOs.Document;
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
            var extension = Path.GetExtension(documentRequestDTO.File.FileName).ToLowerInvariant();

            var fileName = $"{documentId}{extension}";
            var filePath = Path.Combine(fileStorageOptions.BasePath, fileName);

            if (!Directory.Exists(fileStorageOptions.BasePath))
                Directory.CreateDirectory(fileStorageOptions.BasePath);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentRequestDTO.File.CopyToAsync(stream);
                }


                var slug = GenerateSlug.New(documentRequestDTO.Title, documentId);

                var status = await _applicationDbContext.Statuses
                     .FirstOrDefaultAsync(c => c.Id == documentRequestDTO.StatusId);

                var categories = await _applicationDbContext.Categories
                    .Where(c => documentRequestDTO.CategoryIds.Contains(c.Id))
                    .ToListAsync();

                var newDocument = new Models.Document()
                {
                    Id = Guid.NewGuid(),
                    Title = documentRequestDTO.Title,
                    Description = documentRequestDTO.Description,
                    Slug = documentRequestDTO.Title.ToLower().Replace(" ", "-"),
                    PublicationDate = DateTime.UtcNow,
                    FileSize = documentRequestDTO.File.Length,
                    StatusId = documentRequestDTO.StatusId,
                    DocumentTypeId = documentRequestDTO.DocumentTypeId,
                    Categories = _applicationDbContext.Categories
                    .Where(c => documentRequestDTO.CategoryIds.Contains(c.Id))
                    .ToList()
                };


                return  (await _documentAdderRepository.CreateDocument(newDocument))
                    .ToDocumentResponseDTO();
            }
            catch
            {
                if (File.Exists(filePath))
                    
                    await _fileSystems.DeleteAsync(filePath, "documento");

                throw new CreateObjectException("Error al crear el documento.");
            }
        }
    }
}

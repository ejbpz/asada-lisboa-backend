using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.ServiceContracts.Document;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Services.FileSysyem;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Services.Document
{
    public class DocumentAdderService: IDocumentAdderService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IDocumentAdderRepository _documentAdderRepository;
        private readonly IFileSystemManager _fileSystemManager;

        public DocumentAdderService(ApplicationDbContext applicationDbContext, IDocumentAdderRepository documentAdderRepository, IFileSystemManager fileSystemManager)
        {
            _applicationDbContext = applicationDbContext;
            _documentAdderRepository = documentAdderRepository;
            _fileSystemManager = fileSystemManager;

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
                    
                    await _fileSystem.DeleteAsync(filePath, "documento");

                throw new CreateObjectException("Error al crear el documento.");
            }
        }
    }
}

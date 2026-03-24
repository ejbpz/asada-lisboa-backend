using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Document;


namespace AsadaLisboaBackend.Services.Document
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DocumentService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<DocumentResponseDTO> CreateDocument(DocumentRequestDTO documentRequestDTO, FileStorageOptions fileStorageOptions)
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

                _applicationDbContext.Documents.Add(newDocument);
                var affectedRows = await _applicationDbContext.SaveChangesAsync();

                if (affectedRows < 1)
                    throw new NotFoundException("Imagen no encontrada.");

                return newDocument.ToDocumentResponseDTO();
            }
            catch
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw new CreateObjectException("Error al crear la imagen.");
            }
        }
    }
}

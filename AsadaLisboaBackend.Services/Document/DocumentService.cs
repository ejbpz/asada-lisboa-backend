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
                    throw new NotFoundException("Documento no encontrado.");

                return newDocument.ToDocumentResponseDTO();
            }
            catch
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw new CreateObjectException("Error al crear el documento.");
            }
        }

        public async Task<DocumentResponseDTO> UpdateDocument(Guid id, DocumentUpdateRequestDTO documentUpdateRequestDTO, FileStorageOptions fileStorageOptions)
        {


            var document = await _applicationDbContext.Documents
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (document is null)
                throw new NotFoundException("Documento no encontrado.");

            document.Title = documentUpdateRequestDTO.Title;
            document.StatusId = documentUpdateRequestDTO.StatusId;
            document.Description = documentUpdateRequestDTO.Description;

            document.Slug = GenerateSlug.New(documentUpdateRequestDTO.Title, document.Id);

            var categories = await _applicationDbContext.Categories
                .Where(c => documentUpdateRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();

            document.Categories.Clear();
            document.Categories = categories;

            if (documentUpdateRequestDTO.File is null || documentUpdateRequestDTO.File.Length <= 0)
                throw new NotFoundException("Documento no encontrado.");

            var extension = Path.GetExtension(documentUpdateRequestDTO.File.FileName).ToLowerInvariant();
            var newFileName = $"{document.Id}{extension}";
            var newFilePath = Path.Combine(fileStorageOptions.BasePath, newFileName);

            try
            {
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await documentUpdateRequestDTO.File.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath) && document.FilePath != newFilePath)
                    File.Delete(document.FilePath);

                document.FileName = newFileName;
                document.FilePath = newFilePath;
                document.FileSize = documentUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (File.Exists(newFilePath))
                    File.Delete(newFilePath);

                throw new UpdateObjectException("Error al actualizar el documento.");
            }

            var affectedRows = await _applicationDbContext.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Documento no encontrado.");

            return document.ToDocumentResponseDTO();

        }


        public async Task<bool> DeleteDocument(Guid id)
        {
            var document = await _applicationDbContext.Documents
                .FirstOrDefaultAsync(i => i.Id == id);

            if (document is null)
                throw new NotFoundException("Documento no encontrado.");

            if (!string.IsNullOrEmpty(document.FilePath) && File.Exists(document.FilePath))
                File.Delete(document.FilePath);

            _applicationDbContext.Documents.Remove(document);
            var affectedRows = await _applicationDbContext.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Documento no encontrado.");

            return true;
        }
    }
}

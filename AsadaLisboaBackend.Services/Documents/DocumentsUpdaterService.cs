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
    public class DocumentsUpdaterService : IDocumentsUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IDocumentsGetterRepository _documentGetterRepository;
        private readonly IDocumentsUpdaterRepository _documentUpdateRespository;

        public DocumentsUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemsManager fileSystems, IDocumentsGetterRepository documentGetterRepository, IDocumentsUpdaterRepository documentUpdateRespository)
        {
             _fileSystems = fileSystems;
            _applicationDbContext = applicationDbContext;
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

            return (await _documentUpdateRespository.UpdateDocument(document))
                .ToDocumentResponseDTO();
        }

    }

}

    


    
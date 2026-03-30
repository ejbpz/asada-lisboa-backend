using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models.DTOs.Document;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.FileSystem;
using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.RepositoryContracts.Document;

namespace AsadaLisboaBackend.Services.Document
{
    public class DocumentUpdaterService : IDocumentUpdaterService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFileSystemManager _fileSystem;
        private readonly IDocumentGetterRepository _documentGetterRepository;
        private readonly IDocumentUpdateRespository _documentUpdateRespository;

        public DocumentUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemManager _fileSystem, IDocumentGetterRepository documentGetterRepository, IDocumentUpdateRespository documentUpdateRespository)
        {
            _applicationDbContext = applicationDbContext;
             _fileSystem = fileSystem;
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

    


    
using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImagesAdderService : IImagesAdderService
    {
        private readonly IFileSystemManager _fileSystem;
        private readonly IImagesAdderRepository _imagesAdderRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public ImagesAdderService(ApplicationDbContext applicationDbContext, IImagesAdderRepository imagesAdderRepository, IFileSystemManager fileSystem)
        {
            _fileSystem = fileSystem;
            _applicationDbContext = applicationDbContext;
            _imagesAdderRepository = imagesAdderRepository;
        }

        public async Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO)
        {
            if (imageRequestDTO.File is null || imageRequestDTO.File.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var imageId = Guid.NewGuid();

            string? url = string.Empty;

            try
            {
                url = await _fileSystem.SaveAsync(imageRequestDTO.File, "images");

                var fileName = Path.GetFileName(url);
                var filePath = $"images/{fileName}";

                var slug = GenerateSlug.New(imageRequestDTO.Title, imageId);

                var status = await _applicationDbContext.Statuses
                    .FirstOrDefaultAsync(c => c.Id == imageRequestDTO.StatusId);

                var categories = await _applicationDbContext.Categories
                    .Where(c => imageRequestDTO.CategoryIds.Contains(c.Id))
                    .ToListAsync();

                var image = new Models.Image()
                {
                    Id = imageId,
                    Url = url,
                    Slug = slug,
                    Status = status,
                    FilePath = filePath,
                    FileName = fileName,
                    Categories = categories,
                    Title = imageRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
                    StatusId = imageRequestDTO.StatusId,
                    FileSize = imageRequestDTO.File.Length,
                    Description = imageRequestDTO.Description,
                };

                return (await _imagesAdderRepository.CreateImage(image))
                    .ToImageResponseDTO();
            }
            catch
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var fileName = Path.GetFileName(url);
                    await _fileSystem.DeleteAsync(fileName, "images");
                }

                throw new CreateObjectException("Error al crear la imagen.");
            }
        }
    }
}

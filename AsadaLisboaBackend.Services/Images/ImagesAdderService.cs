using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesAdderService : IImagesAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IImagesAdderRepository _imagesAdderRepository;

        public ImagesAdderService(ApplicationDbContext applicationDbContext, IImagesAdderRepository imagesAdderRepository, IFileSystemsManager fileSystems)
        {
            _fileSystems = fileSystems;
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
                url = await _fileSystems.SaveAsync(imageRequestDTO.File, "images");

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
                    await _fileSystems.DeleteAsync(fileName, "images");
                }

                throw new CreateObjectException("Error al crear la imagen.");
            }
        }
    }
}

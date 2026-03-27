using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IFileSystemManager _fileSystem;
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageService(ApplicationDbContext applicationDbContext, IFileSystemManager fileSystem)
        {
            _fileSystem = fileSystem;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO, FileStorageOptions fileStorageOptions)
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

                _applicationDbContext.Images.Add(image);
                await _applicationDbContext.SaveChangesAsync();

                return image.ToImageResponseDTO();
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

        public async Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO, FileStorageOptions fileStorageOptions)
        {
            var image = await _applicationDbContext.Images
                .Include(i => i.Status)
                .Include(i => i.Categories)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            image.Title = imageUpdateRequestDTO.Title;
            image.StatusId = imageUpdateRequestDTO.StatusId;
            image.Description = imageUpdateRequestDTO.Description;

            image.Slug = GenerateSlug.New(imageUpdateRequestDTO.Title, image.Id);

            var categories = await _applicationDbContext.Categories
                .Where(c => imageUpdateRequestDTO.CategoryIds.Contains(c.Id))
                .ToListAsync();

            image.Categories.Clear();
            image.Categories = categories;

            if (imageUpdateRequestDTO.File is null || imageUpdateRequestDTO.File.Length <= 0)
                throw new ArgumentNullException("Error al actualizar la imagen principal.");

            string? newUrl = string.Empty;

            try
            {
                newUrl = await _fileSystem.SaveAsync(imageUpdateRequestDTO.File, "images");

                var newFileName = Path.GetFileName(newUrl);

                if(!string.IsNullOrEmpty(image.FileName))
                    await _fileSystem.DeleteAsync(image.FileName, "images");

                image.Url = newUrl;
                image.FileName = newFileName;
                image.FilePath = $"images/{newFileName}";
                image.FileSize = imageUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (!string.IsNullOrEmpty(newUrl))
                {
                    var fileName = Path.GetFileName(newUrl);
                    await _fileSystem.DeleteAsync(fileName, "images");
                }

                throw new CreateObjectException("Error al actualizar la imagen.");
            }

            await _applicationDbContext.SaveChangesAsync();
            return image.ToImageResponseDTO();
        }

        public async Task<bool> DeleteImage(Guid id)
        {
            var image = await _applicationDbContext.Images
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            if (!string.IsNullOrEmpty(image.FileName))
                await _fileSystem.DeleteAsync(image.FileName, "images");

            _applicationDbContext.Images.Remove(image);
            await _applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}

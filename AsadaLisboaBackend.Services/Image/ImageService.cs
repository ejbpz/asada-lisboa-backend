using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Image;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ImageService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ImageResponseDTO> CreateImage(ImageRequestDTO imageRequestDTO, FileStorageOptions fileStorageOptions)
        {
            if (imageRequestDTO.File is null || imageRequestDTO.File.Length == 0)
                throw new ArgumentException("Archivo inválido.");

            var imageId = Guid.NewGuid();
            var extension = Path.GetExtension(imageRequestDTO.File.FileName).ToLowerInvariant();

            var fileName = $"{imageId}{extension}";
            var filePath = Path.Combine(fileStorageOptions.BasePath, fileName);

            if (!Directory.Exists(fileStorageOptions.BasePath))
                Directory.CreateDirectory(fileStorageOptions.BasePath);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageRequestDTO.File.CopyToAsync(stream);
                }

                var fileUrl = $"{fileStorageOptions.BaseUrl}/{fileName}";
                var slug = GenerateSlug.New(imageRequestDTO.Title, imageId);
                var status = await _applicationDbContext.Statuses
                    .FirstOrDefaultAsync(c => c.Id == imageRequestDTO.StatusId);
                var categories = await _applicationDbContext.Categories
                    .Where(c => imageRequestDTO.CategoryIds.Contains(c.Id))
                    .ToListAsync();

                var image = new Models.Image()
                { 
                    Id = imageId,
                    Slug = slug,          
                    Url = fileUrl,
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
                var affectedRows = await _applicationDbContext.SaveChangesAsync();

                if (affectedRows < 1)
                    throw new NotFoundException("Imagen no encontrada.");

                return image.ToImageResponseDTO();
            }
            catch
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                throw new CreateObjectException("Error al crear la imagen.");
            }
        }

        public async Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO, FileStorageOptions fileStorageOptions)
        {
            var image = await _applicationDbContext.Images
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
                throw new NotFoundException("Imagen no encontrada.");

            var extension = Path.GetExtension(imageUpdateRequestDTO.File.FileName).ToLowerInvariant();
            var newFileName = $"{image.Id}{extension}";
            var newFilePath = Path.Combine(fileStorageOptions.BasePath, newFileName);

            try
            {
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await imageUpdateRequestDTO.File.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(image.FilePath) && File.Exists(image.FilePath) && image.FilePath != newFilePath)
                    File.Delete(image.FilePath);

                image.FileName = newFileName;
                image.FilePath = newFilePath;
                image.Url = $"{fileStorageOptions.BaseUrl}{newFileName}";
                image.FileSize = imageUpdateRequestDTO.File.Length;
            }
            catch
            {
                if (File.Exists(newFilePath))
                    File.Delete(newFilePath);

                throw new UpdateObjectException("Error al actualizar la imagen.");
            }

            var affectedRows = await _applicationDbContext.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Imagen no encontrada.");

            return image.ToImageResponseDTO();
        }

        public async Task<bool> DeleteImage(Guid id)
        {
            var image = await _applicationDbContext.Images
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            if (!string.IsNullOrEmpty(image.FilePath) && File.Exists(image.FilePath))
                File.Delete(image.FilePath);

            _applicationDbContext.Images.Remove(image);
            var affectedRows = await _applicationDbContext.SaveChangesAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Imagen no encontrada.");

            return true;
        }
    }
}

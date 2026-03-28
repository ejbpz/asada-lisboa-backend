using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImagesUpdaterService : IImagesUpdaterService
    {
        private readonly IFileSystemManager _fileSystem;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IImagesGetterRepository _imagesGetterRespository;
        private readonly IImagesUpdaterRepository _imagesUpdaterRepository;

        public ImagesUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemManager fileSystem, IImagesUpdaterRepository imagesUpdaterRepository, IImagesGetterRepository imagesGetterRespository)
        {
            _fileSystem = fileSystem;
            _applicationDbContext = applicationDbContext;
            _imagesUpdaterRepository = imagesUpdaterRepository;
            _imagesGetterRespository = imagesGetterRespository;
        }

        public async Task<ImageResponseDTO> UpdateImage(Guid id, ImageUpdateRequestDTO imageUpdateRequestDTO)
        {
            var image = await _imagesGetterRespository.GetImage(id);

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

            return (await _imagesUpdaterRepository.UpdateImage(image))
                .ToImageResponseDTO();
        }
    }
}

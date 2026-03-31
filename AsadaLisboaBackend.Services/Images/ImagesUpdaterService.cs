using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesUpdaterService : IImagesUpdaterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly IImagesGetterRepository _imagesGetterRespository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IImagesUpdaterRepository _imagesUpdaterRepository;

        public ImagesUpdaterService(ApplicationDbContext applicationDbContext, IFileSystemsManager fileSystems, IImagesUpdaterRepository imagesUpdaterRepository, IImagesGetterRepository imagesGetterRespository, ICategoriesGetterService categoriesGetterService)
        {
            _fileSystems = fileSystems;
            _categoriesGetterService = categoriesGetterService;
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

            image.Categories = await _categoriesGetterService.ToCreateCategories(imageUpdateRequestDTO.Categories);

            if (imageUpdateRequestDTO.File is null || imageUpdateRequestDTO.File.Length <= 0)
                throw new ArgumentNullException("Error al actualizar la imagen.");

            string? newUrl = string.Empty;

            try
            {
                newUrl = await _fileSystems.SaveAsync(imageUpdateRequestDTO.File, "images");

                var newFileName = Path.GetFileName(newUrl);

                if (!string.IsNullOrEmpty(image.FilePath) && File.Exists(image.FilePath) && image.FilePath != newUrl)
                    File.Delete(image.FilePath);

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
                    await _fileSystems.DeleteAsync(fileName, "images");
                }

                throw new CreateObjectException("Error al actualizar la imagen.");
            }

            return (await _imagesUpdaterRepository.UpdateImage(image))
                .ToImageResponseDTO();
        }
    }
}
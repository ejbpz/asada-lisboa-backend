using AsadaLisboaBackend.Models.DTOs.Image;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Utils.SlugGeneration;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Statuses;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesAdderService : IImagesAdderService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly IImagesAdderRepository _imagesAdderRepository;
        private readonly ICategoriesGetterService _categoriesGetterService;
        private readonly IStatusesGetterRepository _statusesGetterRepository;

        public ImagesAdderService(IImagesAdderRepository imagesAdderRepository, IFileSystemsManager fileSystems, ICategoriesGetterService categoriesGetterService, IStatusesGetterRepository statusesGetterRepository)
        {
            _fileSystems = fileSystems;
            _imagesAdderRepository = imagesAdderRepository;
            _categoriesGetterService = categoriesGetterService;
            _statusesGetterRepository = statusesGetterRepository;
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

                var status = await _statusesGetterRepository.GetStatus(imageRequestDTO.StatusId);

                var categories = await _categoriesGetterService.ToCreateCategories(imageRequestDTO.Categories);

                var image = new Models.Image()
                {
                    Id = imageId,
                    Url = url,
                    Slug = slug,
                    FilePath = filePath,
                    FileName = fileName,
                    StatusId = status.Id,
                    Categories = categories,
                    Title = imageRequestDTO.Title,
                    PublicationDate = DateTime.UtcNow,
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

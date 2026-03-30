using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Images;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystems;

namespace AsadaLisboaBackend.Services.Images
{
    public class ImagesDeleterService : IImagesDeleterService
    {
        private readonly IFileSystemsManager _fileSystems;
        private readonly IImagesGetterRepository _imagesGetterRepository;
        private readonly IImagesDeleterRepository _imagesDeleterRepository;

        public ImagesDeleterService(IFileSystemsManager fileSystems, IImagesDeleterRepository imagesDeleterRepository, IImagesGetterRepository imagesGetterRepository)
        {
            _fileSystems = fileSystems;
            _imagesGetterRepository = imagesGetterRepository;
            _imagesDeleterRepository = imagesDeleterRepository;
        }

        public async Task DeleteImage(Guid id)
        {
            var image = await _imagesGetterRepository.GetImage(id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            if (!string.IsNullOrEmpty(image.FileName))
                await _fileSystems.DeleteAsync(image.FileName, "images");

            await _imagesDeleterRepository.DeleteImage(id);
        }
    }
}

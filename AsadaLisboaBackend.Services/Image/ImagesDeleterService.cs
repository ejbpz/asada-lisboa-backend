using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Image;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.ServiceContracts.FileSystem;

namespace AsadaLisboaBackend.Services.Image
{
    public class ImagesDeleterService : IImagesDeleterService
    {
        private readonly IFileSystemManager _fileSystem;
        private readonly IImagesGetterRepository _imagesGetterRepository;
        private readonly IImagesDeleterRepository _imagesDeleterRepository;

        public ImagesDeleterService(IFileSystemManager fileSystem, IImagesDeleterRepository imagesDeleterRepository, IImagesGetterRepository imagesGetterRepository)
        {
            _fileSystem = fileSystem;
            _imagesGetterRepository = imagesGetterRepository;
            _imagesDeleterRepository = imagesDeleterRepository;
        }

        public async Task DeleteImage(Guid id)
        {
            var image = await _imagesGetterRepository.GetImage(id);

            if (image is null)
                throw new NotFoundException("Imagen no encontrada.");

            if (!string.IsNullOrEmpty(image.FileName))
                await _fileSystem.DeleteAsync(image.FileName, "images");

            await _imagesDeleterRepository.DeleteImage(id);
        }
    }
}

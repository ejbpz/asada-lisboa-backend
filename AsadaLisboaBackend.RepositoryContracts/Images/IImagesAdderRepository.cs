using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.Images
{
    public interface IImagesAdderRepository
    {
        public Task<Image> CreateImage(Image image);
    }
}

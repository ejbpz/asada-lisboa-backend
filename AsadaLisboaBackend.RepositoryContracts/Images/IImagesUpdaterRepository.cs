using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.Images
{
    public interface IImagesUpdaterRepository
    {
        public Task<Image> UpdateImage(Image image);
    }
}

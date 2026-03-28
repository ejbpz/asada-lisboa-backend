namespace AsadaLisboaBackend.RepositoryContracts.Images
{
    public interface IImagesDeleterRepository
    {
        public Task DeleteImage(Guid id);
    }
}

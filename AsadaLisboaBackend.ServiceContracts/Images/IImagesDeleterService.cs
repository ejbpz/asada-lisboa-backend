namespace AsadaLisboaBackend.ServiceContracts.Images
{
    public interface IImagesDeleterService
    {
        public Task DeleteImage(Guid id);
    }
}

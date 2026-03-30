namespace AsadaLisboaBackend.ServiceContracts.Image
{
    public interface IImagesDeleterService
    {
        public Task DeleteImage(Guid id);
    }
}

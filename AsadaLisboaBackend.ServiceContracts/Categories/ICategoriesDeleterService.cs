namespace AsadaLisboaBackend.ServiceContracts.Categories
{
    public interface ICategoriesDeleterService
    {
        public Task DeleteCategory(Guid id);
    }
}

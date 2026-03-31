namespace AsadaLisboaBackend.RepositoryContracts.Categories
{
    public interface ICategoriesDeleterRepository
    {
        public Task DeleteCategory(Guid id);
    }
}

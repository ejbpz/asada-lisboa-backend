using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.Categories
{
    public interface ICategoriesAdderRepository
    {
        public Task AddCategories(List<Category> categoryResponseDTO);
    }
}

using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories
{
    public class CategoriesDeleterService : ICategoriesDeleterService
    {
        private readonly ICategoriesDeleterRepository _categoriesDeleterRepository;

        public CategoriesDeleterService(ICategoriesDeleterRepository categoriesDeleterRepository)
        {
            _categoriesDeleterRepository = categoriesDeleterRepository;
        }

        public async Task DeleteCategory(Guid id)
        {
            await _categoriesDeleterRepository.DeleteCategory(id);
        }
    }
}

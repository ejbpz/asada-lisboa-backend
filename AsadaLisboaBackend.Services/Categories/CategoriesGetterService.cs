using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories
{
    public class CategoriesGetterService : ICategoriesGetterService
    {
        private readonly ICategoriesGetterRepository _categoriesGetterRepository;

        public CategoriesGetterService(ICategoriesGetterRepository categoriesGetterRepository)
        {
            _categoriesGetterRepository = categoriesGetterRepository;
        }

        public async Task<List<CategoryResponseDTO>> GetCategories(ObjectTypeEnum objectTypeEnum)
        {
            return await _categoriesGetterRepository.GetCategories(objectTypeEnum);
        }

        public async Task<List<CategoryResponseDTO>> SearchCategories(ObjectTypeEnum objectTypeEnum, string search)
        {
            search = search.Trim().ToLower();
            return await _categoriesGetterRepository.SearchCategories(objectTypeEnum, search);
        }
    }
}

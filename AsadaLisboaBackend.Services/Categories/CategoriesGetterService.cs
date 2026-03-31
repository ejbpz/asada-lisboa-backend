using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories
{
    public class CategoriesGetterService : ICategoriesGetterService
    {
        private readonly ICategoriesAdderRepository _categoriesAdderRepository;
        private readonly ICategoriesGetterRepository _categoriesGetterRepository;

        public CategoriesGetterService(ICategoriesGetterRepository categoriesGetterRepository, ICategoriesAdderRepository categoriesAdderRepository)
        {
            _categoriesAdderRepository = categoriesAdderRepository;
            _categoriesGetterRepository = categoriesGetterRepository;
        }

        public async Task<List<CategoryResponseDTO>> GetCategories()
        {
            return await _categoriesGetterRepository.GetCategories();
        }

        public async Task<List<CategoryResponseDTO>> SearchCategories(string search)
        {
            search = search.Trim().ToLower();
            return await _categoriesGetterRepository.SearchCategories(search);
        }

        public async Task<List<Category>> ToCreateCategories(List<CategoryRequestDTO> categories)
        {
            var categoriesWithoutId = await NoIdCategories(categories);
            var categoriesWithId = await IdCategories(categories);

            var names = categories.Select(c => c.Name.Trim().ToLower()).ToList();

            var existingByName = await _categoriesGetterRepository.NoRepeatNames(categories, names);
            var toCreate = await _categoriesGetterRepository.ToCreateCategories(existingByName);

            await _categoriesAdderRepository.AddCategories(toCreate);

            var categoriesToReturn = new List<Guid>();

            categoriesToReturn.AddRange(categoriesWithId.Select(c => c.Id));
            categoriesToReturn.AddRange(existingByName.Select(c => c.Id));
            categoriesToReturn.AddRange(toCreate.Select(c => c.Id));

            return categoriesToReturn
                .Distinct()
                .Select(id => new Category { Id = id })
                .ToList();
        }

        private async Task<List<CategoryResponseDTO>> NoIdCategories(List<CategoryRequestDTO> categoryRequestDTO)
        {
            var noIdCategoriesList = categoryRequestDTO
                 .Where(c => c.Id.HasValue != true)
                 .Select(c => new CategoryResponseDTO { Name = c.Name })
                 .ToList();

            await Task.CompletedTask;
            return noIdCategoriesList;
        }

        private async Task<List<CategoryResponseDTO>> IdCategories(List<CategoryRequestDTO> categoryRequestDTO)
        {
            var noIdCategoriesList = categoryRequestDTO
                 .Where(c => c.Id.HasValue == true)
                 .Select(c => new CategoryResponseDTO { Id = c.Id!.Value, Name = c.Name })
                 .ToList();

            await Task.CompletedTask;
            return noIdCategoriesList;
        }
    }
}

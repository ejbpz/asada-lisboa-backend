using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories
{
    public class CategoriesGetterService : ICategoriesGetterService
    {
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly ICategoriesAdderRepository _categoriesAdderRepository;
        private readonly ICategoriesGetterRepository _categoriesGetterRepository;

        public CategoriesGetterService(ICategoriesGetterRepository categoriesGetterRepository, ICategoriesAdderRepository categoriesAdderRepository, IMemoryCachesService memoryCachesService)
        {
            _memoryCachesService = memoryCachesService;
            _categoriesAdderRepository = categoriesAdderRepository;
            _categoriesGetterRepository = categoriesGetterRepository;
        }

        public async Task<List<CategoryResponseDTO>> GetCategories()
        {
            return await _memoryCachesService.GetOrCreateCache<List<CategoryResponseDTO>>(
                key: Constants.CACHE_CATEGORIES,
                create: () => _categoriesGetterRepository.GetCategories(),
                time: TimeSpan.FromMinutes(2));
        }

        public async Task<List<CategoryResponseDTO>> SearchCategories(string search)
        {
            search = search.Trim().ToLower();

            return await _memoryCachesService.GetOrCreateCacheList<List<CategoryResponseDTO>>(
                resource: Constants.CACHE_CATEGORIES,
                request: search,
                create: () => _categoriesGetterRepository.SearchCategories(search),
                time: TimeSpan.FromMinutes(5));
        }

        public async Task<HashSet<Category>> ToCreateCategories(List<CategoryRequestDTO> categories)
        {
            var categoriesWithoutId = await NoIdCategories(categories);
            var categoriesWithId = await IdCategories(categories);
            
            var names = categoriesWithoutId
                .Select(c => c.Name.Trim().ToLower())
                .ToList();

            var existingByName = await _categoriesGetterRepository.NoRepeatNames(names);

            var existingNames = existingByName
                .Select(c => c.Name.Trim().ToLower())
                .ToList();

            var toCreate = _categoriesGetterRepository.ToCreateCategories(categoriesWithoutId, existingNames);

            if (toCreate.Any())
                await _categoriesAdderRepository.AddCategories(toCreate);

            var categoriesToReturn = new List<Guid>();

            categoriesToReturn.AddRange(categoriesWithId.Select(c => c.Id));
            categoriesToReturn.AddRange(existingByName.Select(c => c.Id));
            categoriesToReturn.AddRange(toCreate.Select(c => c.Id));

            return categoriesToReturn
                .Distinct()
                .Select(id => new Category { Id = id })
                .ToHashSet();
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

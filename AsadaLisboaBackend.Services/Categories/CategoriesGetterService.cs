using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.ServiceContracts.Categories;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Services.Categories;

public class CategoriesGetterService(
    ICategoriesGetterRepository categoriesGetterRepository,
    ICategoriesAdderRepository categoriesAdderRepository,
    IMemoryCachesService memoryCachesService)
    : ICategoriesGetterService
{
    public async Task<List<CategoryResponseDto>> GetCategories()
    {
        return await memoryCachesService.GetOrCreateCache(
            key: Constants.CACHE_CATEGORIES,
            create: categoriesGetterRepository.GetCategories,
            time: TimeSpan.FromMinutes(2));
    }

    public async Task<List<CategoryResponseDto>> SearchCategories(string search)
    {
        search = search.Trim().ToLower();

        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_CATEGORIES,
            request: search,
            create: () => categoriesGetterRepository.SearchCategories(search),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<HashSet<Category>> ToCreateCategories(List<CategoryRequestDto> categories)
    {
        var categoriesWithoutId = await NoIdCategories(categories);
        var categoriesWithId = await IdCategories(categories);
            
        var names = categoriesWithoutId
            .Select(c => c.Name.Trim().ToLower())
            .ToList();

        var existingByName = await categoriesGetterRepository.NoRepeatNames(names);

        var existingNames = existingByName
            .Select(c => c.Name.Trim().ToLower())
            .ToList();

        var toCreate = categoriesGetterRepository.ToCreateCategories(categoriesWithoutId, existingNames);

        if (toCreate.Any())
            await categoriesAdderRepository.AddCategories(toCreate);

        var categoriesToReturn = new List<Guid>();

        categoriesToReturn.AddRange(categoriesWithId.Select(c => c.Id));
        categoriesToReturn.AddRange(existingByName.Select(c => c.Id));
        categoriesToReturn.AddRange(toCreate.Select(c => c.Id));

        return categoriesToReturn
            .Distinct()
            .Select(id => new Category { Id = id })
            .ToHashSet();
    }

    private async Task<List<CategoryResponseDto>> NoIdCategories(List<CategoryRequestDto> categoryRequestDto)
    {
        var noIdCategoriesList = categoryRequestDto
            .Where(c => c.Id.HasValue != true)
            .Select(c => new CategoryResponseDto { Name = c.Name ?? "" })
            .ToList();

        await Task.CompletedTask;
        return noIdCategoriesList;
    }

    private async Task<List<CategoryResponseDto>> IdCategories(List<CategoryRequestDto> categoryRequestDto)
    {
        var noIdCategoriesList = categoryRequestDto
            .Where(c => c.Id.HasValue)
            .Select(c => new CategoryResponseDto { Id = c.Id!.Value, Name = c.Name ?? ""})
            .ToList();

        await Task.CompletedTask;
        return noIdCategoriesList;
    }
}
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.RepositoryContracts.Categories;

public interface ICategoriesGetterRepository
{
    public Task<List<CategoryResponseDto>> GetCategories();
    public Task<List<CategoryResponseDto>> SearchCategories(string search); 
    public Task<List<CategoryResponseDto>> NoRepeatNames(List<string> names);
    public List<Category> ToCreateCategories(List<CategoryResponseDto> categoriesWithoutId, List<string> existingNames);
}
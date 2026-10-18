using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.ServiceContracts.Categories;

public interface ICategoriesGetterService
{
    public Task<List<CategoryResponseDto>> GetCategories();
    public Task<List<CategoryResponseDto>> SearchCategories(string search);
    public Task<HashSet<Category>> ToCreateCategories(List<CategoryRequestDto> categories);
}
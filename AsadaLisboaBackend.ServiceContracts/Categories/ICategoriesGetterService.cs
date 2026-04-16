using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.ServiceContracts.Categories
{
    public interface ICategoriesGetterService
    {
        public Task<List<CategoryResponseDTO>> GetCategories();
        public Task<List<CategoryResponseDTO>> SearchCategories(string search);
        public Task<HashSet<Category>> ToCreateCategories(List<CategoryRequestDTO> categories);
    }
}

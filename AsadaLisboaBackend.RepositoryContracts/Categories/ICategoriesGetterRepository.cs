using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.RepositoryContracts.Categories
{
    public interface ICategoriesGetterRepository
    {
        public Task<List<CategoryResponseDTO>> GetCategories();
        public Task<List<CategoryResponseDTO>> SearchCategories(string search); 
        public Task<List<CategoryResponseDTO>> NoRepeatNames(List<CategoryRequestDTO> categoryRequestDTO, List<string> names);
        public Task<List<Category>> ToCreateCategories(List<CategoryResponseDTO> categoryResponseDTO);
    }
}

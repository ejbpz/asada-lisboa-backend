using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.ServiceContracts.Categories
{
    public interface ICategoriesGetterService
    {
        public Task<List<CategoryResponseDTO>> GetCategories(ObjectTypeEnum objectTypeEnum);
        public Task<List<Category>> ToCreateCategories(List<CategoryRequestDTO> categories);
        public Task<List<CategoryResponseDTO>> SearchCategories(ObjectTypeEnum objectTypeEnum, string search);
    }
}

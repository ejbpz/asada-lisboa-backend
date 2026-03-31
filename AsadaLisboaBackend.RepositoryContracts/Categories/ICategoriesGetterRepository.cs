using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Category;

namespace AsadaLisboaBackend.RepositoryContracts.Categories
{
    public interface ICategoriesGetterRepository
    {
        public Task<List<CategoryResponseDTO>> GetCategories(ObjectTypeEnum objectTypeEnum);
        public Task<List<CategoryResponseDTO>> SearchCategories(ObjectTypeEnum objectTypeEnum, string search);
    }
}

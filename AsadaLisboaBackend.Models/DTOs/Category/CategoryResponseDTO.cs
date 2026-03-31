using System.Linq.Expressions;

namespace AsadaLisboaBackend.Models.DTOs.Category
{
    public class CategoryResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public static partial class CategoryExtensions
    {
        public static Expression<Func<Models.Category, CategoryResponseDTO>> MapCategoryResponseDTO()
        {
            return category => new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static CategoryResponseDTO ToCategoryResponseDTO(this Models.Category category)
        {
            return new CategoryResponseDTO()
            {
                Id = category.Id,
                Name = category.Name,
            };
        }
    }
}

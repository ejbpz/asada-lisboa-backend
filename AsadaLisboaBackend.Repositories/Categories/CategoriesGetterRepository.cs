using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.Enums;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;
using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.Repositories.Categories
{
    public class CategoriesGetterRepository : ICategoriesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryResponseDTO>> SearchCategories(ObjectTypeEnum objectTypeEnum, string search)
        {
            var query = _context.Categories
                .AsNoTracking()
                .AsQueryable();

            query = objectTypeEnum switch
            {
                ObjectTypeEnum.New => query.Include(c => c.News),
                ObjectTypeEnum.Image => query.Include(c => c.Images),
                ObjectTypeEnum.Document => query.Include(c => c.Documents),
                _ => query
            };

            return await query
                .Where(c => c.Name.ToLower().Trim().Contains(search))
                .OrderBy(c => c.Name)
                .Take(10)
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public async Task<List<CategoryResponseDTO>> GetCategories(ObjectTypeEnum objectTypeEnum)
        {
            var query = _context.Categories
                .AsNoTracking()
                .AsQueryable();

            query = objectTypeEnum switch
            {
                ObjectTypeEnum.New => query.Include(c => c.News),
                ObjectTypeEnum.Image => query.Include(c => c.Images),
                ObjectTypeEnum.Document => query.Include(c => c.Documents),
                _ => query
            };

            return await query
                .OrderBy(c => c.Name)
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public async Task<List<CategoryResponseDTO>> NoRepeatNames(List<CategoryRequestDTO> categoryRequestDTO, List<string> names)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => names.Contains(c.Name.Trim().ToLower()))
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public async Task<List<Category>> ToCreateCategories(List<CategoryResponseDTO> categoryResponseDTO)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => !categoryResponseDTO.Any(r => r.Name.Trim().ToLower() == c.Name.Trim().ToLower()))
                .Select(c => new Category { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }
    }
}

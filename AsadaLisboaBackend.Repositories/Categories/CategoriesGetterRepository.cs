using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DTOs.Category;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Repositories.Categories
{
    public class CategoriesGetterRepository : ICategoriesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryResponseDTO>> SearchCategories(string search)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.News)
                .Include(c => c.Images)
                .Include(c => c.Documents)
                .Distinct()
                .Where(c => c.Name.ToLower().Trim().Contains(search))
                .OrderBy(c => c.Name)
                .Take(10)
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public async Task<List<CategoryResponseDTO>> GetCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.News)
                .Include(c => c.Images)
                .Include(c => c.Documents)
                .Distinct()
                .OrderBy(c => c.Name)
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public async Task<List<CategoryResponseDTO>> NoRepeatNames(List<string> names)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => names.Contains(c.Name.Trim().ToLower()))
                .Select(CategoryExtensions.MapCategoryResponseDTO())
                .ToListAsync();
        }

        public List<Category> ToCreateCategories(List<CategoryResponseDTO> categoriesWithoutId, List<string> existingNames)
        {
            return categoriesWithoutId
                .Where(c => !existingNames.Contains(c.Name.Trim().ToLower()))
                .Select(c => new Category { Id = Guid.NewGuid(), Name = c.Name.Trim() })
                .ToList();
        }
    }
}

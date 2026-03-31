using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Repositories.Categories
{
    public class CategoriesAdderRepository : ICategoriesAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCategories(List<Category> categories)
        {
            _context.Categories.AddRange(categories);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al agregar las categorías.");
        }
    }
}

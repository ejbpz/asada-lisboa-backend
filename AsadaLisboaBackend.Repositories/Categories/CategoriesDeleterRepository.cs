using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Repositories.Categories
{
    public class CategoriesDeleterRepository : ICategoriesDeleterRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesDeleterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteCategory(Guid id)
        {
            try
            {
                _context.Categories.Remove(new Category { Id = id });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 547)
                    throw new InUsedException("Esta categoría está siendo usada por otro elemento.");

                throw;
            }
        }
    }
}

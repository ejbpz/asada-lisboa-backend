using Microsoft.EntityFrameworkCore;
using Npgsql;
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
                if (ex.InnerException is PostgresException postgresException && postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
                    throw new InUsedException("Esta categoría está siendo usada por otro elemento.");

                throw;
            }
        }
    }
}

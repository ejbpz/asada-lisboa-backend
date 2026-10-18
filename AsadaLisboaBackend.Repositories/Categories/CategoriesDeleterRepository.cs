using Microsoft.EntityFrameworkCore;
using Npgsql;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Repositories.Categories;

public class CategoriesDeleterRepository(ApplicationDbContext context) : ICategoriesDeleterRepository
{
    public async Task DeleteCategory(Guid id)
    {
        try
        {
            context.Categories.Remove(new Category { Id = id });
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.ForeignKeyViolation })
                throw new InUsedException("Esta categoría está siendo usada por otro elemento.");

            throw;
        }
    }
}
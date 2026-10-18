using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Categories;

namespace AsadaLisboaBackend.Repositories.Categories;

public class CategoriesAdderRepository(ApplicationDbContext context) : ICategoriesAdderRepository
{
    public async Task AddCategories(List<Category> categories)
    {
        context.Categories.AddRange(categories);
        var affectedRows = await context.SaveChangesAsync();

        if (affectedRows < 1)
            throw new CreateObjectException("Error al agregar las categorías.");
    }
}
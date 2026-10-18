using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News;

public class NewsAdderRepository(ApplicationDbContext context) : INewsAdderRepository
{
    public async Task<New> CreateNew(New newModel)
    {
        var categoryIds = newModel.Categories
            .Select(c => c.Id)
            .ToList();

        var categoriesFromDb = await context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        newModel.Categories = categoriesFromDb;

        context.News.Add(newModel);
        var affectedRows = await context.SaveChangesAsync();

        if (affectedRows < 1)
            throw new CreateObjectException("Error al crear la noticia.");

        return await context.News
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .FirstAsync(n => n.Id == newModel.Id);
    }
}
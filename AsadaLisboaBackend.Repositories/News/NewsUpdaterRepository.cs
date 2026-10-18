using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News;

public class NewsUpdaterRepository(ApplicationDbContext context) : INewsUpdaterRepository
{
    public async Task<New> UpdateNew(Guid id, New newModel)
    {
        var existingNew = await context.News
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .FirstAsync(d => d.Id == id);

        existingNew.Slug = newModel.Slug;
        existingNew.Title = newModel.Title;
        existingNew.StatusId = newModel.StatusId;
        existingNew.ImageUrl = newModel.ImageUrl;
        existingNew.FileName = newModel.FileName;
        existingNew.FilePath = newModel.FilePath;
        existingNew.Description = newModel.Description;
        existingNew.LastEditionDate = newModel.LastEditionDate;

        var categoryIds = newModel.Categories
            .Select(c => c.Id)
            .ToList();

        var categoriesFromDb = await context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        existingNew.Categories.Clear();
        existingNew.Categories = categoriesFromDb;

        var affectedRows = await context.SaveChangesAsync();

        return affectedRows < 1 ? throw new UpdateObjectException("Error al modificar la noticia.") : existingNew;
    }
}
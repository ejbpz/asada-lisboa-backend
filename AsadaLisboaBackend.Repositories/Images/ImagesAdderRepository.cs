using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images;

public class ImagesAdderRepository(ApplicationDbContext context) : IImagesAdderRepository
{
    public async Task<Image> CreateImage(Image image)
    {
        var categoryIds = image.Categories
            .Select(c => c.Id)
            .ToList();

        var categoriesFromDb = await context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        image.Categories = categoriesFromDb;

        context.Images.Add(image);
        var affectedRows = await context.SaveChangesAsync();

        if (affectedRows < 1)
            throw new CreateObjectException("Error al agregar la imagen.");

        return await context.Images
            .Include(i => i.Status)
            .Include(i => i.Categories)
            .FirstAsync(i => i.Id == image.Id);
    }
}
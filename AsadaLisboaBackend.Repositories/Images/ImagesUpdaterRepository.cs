using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images;

public class ImagesUpdaterRepository(ApplicationDbContext context) : IImagesUpdaterRepository
{
    public async Task<Image> UpdateImage(Image image)
    {
        var existingImage = await context.Images
            .Include(d => d.Status)
            .Include(d => d.Categories)
            .FirstAsync(d => d.Id == image.Id);

        existingImage.Url = image.Url;
        existingImage.Slug = image.Slug;
        existingImage.Title = image.Title;
        existingImage.StatusId = image.StatusId;
        existingImage.FileSize = image.FileSize;
        existingImage.FilePath = image.FilePath;
        existingImage.FileName = image.FileName;
        existingImage.Description = image.Description;

        var categoryIds = image.Categories
            .Select(c => c.Id)
            .ToList();

        var categoriesFromDb = await context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        existingImage.Categories.Clear();
        existingImage.Categories = categoriesFromDb;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new UpdateObjectException("Error al modificar la imagen.", ex);
        }

        return existingImage;
    }
}
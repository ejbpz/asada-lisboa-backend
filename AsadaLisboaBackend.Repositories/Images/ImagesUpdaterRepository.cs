using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Repositories.Images
{
    public class ImagesUpdaterRepository : IImagesUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public ImagesUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> UpdateImage(Image image)
        {
            var existingImage = await _context.Images
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

            var categoriesFromDb = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            existingImage.Categories.Clear();
            existingImage.Categories = categoriesFromDb;

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar la imagen.");

            return existingImage;
        }
    }
}

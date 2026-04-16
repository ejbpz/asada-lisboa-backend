using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;
using AsadaLisboaBackend.Services.Exceptions;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Repositories.Images
{
    public class ImagesAdderRepository : IImagesAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public ImagesAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> CreateImage(Image image)
        {
            var categoryIds = image.Categories
                .Select(c => c.Id)
                .ToList();

            var categoriesFromDb = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            image.Categories = categoriesFromDb;

            _context.Images.Add(image);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al agregar la imagen.");

            return await _context.Images
                .Include(i => i.Status)
                .Include(i => i.Categories)
                .FirstAsync(i => i.Id == image.Id);
        }
    }
}

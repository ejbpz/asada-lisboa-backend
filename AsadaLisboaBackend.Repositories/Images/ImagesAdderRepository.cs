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
            foreach (var category in image.Categories)
            {
                _context.Categories.Attach(category);
            }

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

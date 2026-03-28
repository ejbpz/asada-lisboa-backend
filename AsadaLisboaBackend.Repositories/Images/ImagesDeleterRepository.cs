using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

namespace AsadaLisboaBackend.Repositories.Images
{
    public class ImagesDeleterRepository : IImagesDeleterRepository
    {
        private readonly ApplicationDbContext _context;

        public ImagesDeleterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteImage(Guid id)
        {
            var affectedRows = await _context.Images
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Error al eliminar la imagen.");
        }
    }
}

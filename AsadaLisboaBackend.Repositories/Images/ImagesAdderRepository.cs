using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

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
            _context.Images.Add(image);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al agregar la imagen.");    
            
            return image;
        }
    }
}

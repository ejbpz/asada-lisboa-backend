using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Images;

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
            _context.Attach(image);

            _context.Entry(image).Property(n => n.Url).IsModified = false;
            _context.Entry(image).Property(n => n.Slug).IsModified = false;
            _context.Entry(image).Property(n => n.Title).IsModified = false;
            _context.Entry(image).Property(n => n.StatusId).IsModified = false;
            _context.Entry(image).Property(n => n.FileName).IsModified = false;
            _context.Entry(image).Property(n => n.FilePath).IsModified = false;
            _context.Entry(image).Property(n => n.FileSize).IsModified = false;
            _context.Entry(image).Property(n => n.Categories).IsModified = false;
            _context.Entry(image).Property(n => n.Description).IsModified = false;

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar la imagen.");

            return new Image()
            {
                Id = image.Id,
                Url = image.Url,
                Slug = image.Slug,
                Title = image.Title,
                Status = image.Status,
                FileName = image.FileName,
                FilePath = image.FilePath,
                StatusId = image.StatusId,
                FileSize = image.FileSize,
                Categories = image.Categories,
                Description = image.Description,
                PublicationDate = image.PublicationDate,
            };
        }
    }
}

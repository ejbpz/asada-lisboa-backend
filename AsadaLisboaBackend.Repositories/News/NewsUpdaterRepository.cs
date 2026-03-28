using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

namespace AsadaLisboaBackend.Repositories.News
{
    public class NewsUpdaterRepository : INewsUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<New> UpdateNew(Guid id, New newModel)
        {
            _context.Attach(newModel);

            _context.Entry(newModel).Property(n => n.Slug).IsModified = true;
            _context.Entry(newModel).Property(n => n.Title).IsModified = true;
            _context.Entry(newModel).Property(n => n.StatusId).IsModified = true;
            _context.Entry(newModel).Property(n => n.ImageUrl).IsModified = true;
            _context.Entry(newModel).Property(n => n.FileName).IsModified = true;
            _context.Entry(newModel).Property(n => n.FilePath).IsModified = true;
            _context.Entry(newModel).Property(n => n.Categories).IsModified = true;
            _context.Entry(newModel).Property(n => n.Description).IsModified = true;
            _context.Entry(newModel).Property(n => n.LastEditionDate).IsModified = true;

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar la noticia.");

            return new New()
            {
                Id = id,
                Slug = newModel.Slug,
                Title = newModel.Title,
                Status = newModel.Status,
                ImageUrl = newModel.ImageUrl,
                FileName = newModel.FileName,
                FilePath = newModel.FilePath,
                StatusId = newModel.StatusId,
                Categories = newModel.Categories,
                Description = newModel.Description,
                LastEditionDate = newModel.LastEditionDate,
                PublicationDate = newModel.PublicationDate,
            };
        }
    }
}

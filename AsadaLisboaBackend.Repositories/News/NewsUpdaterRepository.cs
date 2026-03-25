using Microsoft.EntityFrameworkCore;
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
            var affectedRows = await _context.News
                .Where(n => n.Id == id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(n => n.Slug, newModel.Slug)
                    .SetProperty(n => n.Title, newModel.Title)
                    .SetProperty(n => n.ImageUrl, newModel.ImageUrl)
                    .SetProperty(n => n.FileName, newModel.FileName)
                    .SetProperty(n => n.FilePath, newModel.FilePath)
                    .SetProperty(n => n.StatusId, newModel.StatusId)
                    .SetProperty(n => n.Categories, newModel.Categories)
                    .SetProperty(n => n.Description, newModel.Description)
                    .SetProperty(n => n.LastEditionDate, newModel.LastEditionDate));

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

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
            var existingNew = await _context.News
                .Include(d => d.Status)
                .Include(d => d.Categories)
                .FirstAsync(d => d.Id == id);

            existingNew.Slug = newModel.Slug;
            existingNew.Title = newModel.Title;
            existingNew.StatusId = newModel.StatusId;
            existingNew.ImageUrl = newModel.ImageUrl;
            existingNew.FileName = newModel.FileName;
            existingNew.FilePath = newModel.FilePath;
            existingNew.Description = newModel.Description;
            existingNew.LastEditionDate = newModel.LastEditionDate;

            var categoryIds = newModel.Categories
                .Select(c => c.Id)
                .ToList();

            var categoriesFromDb = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            existingNew.Categories.Clear();
            existingNew.Categories = categoriesFromDb;

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar la noticia.");

            return existingNew;
        }
    }
}

using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;
using AsadaLisboaBackend.Services.Exceptions;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.EntityFrameworkCore;

namespace AsadaLisboaBackend.Repositories.News
{
    public class NewsAdderRepository : INewsAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public NewsAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<New> CreateNew(New newModel)
        {
            var categoryIds = newModel.Categories
                .Select(c => c.Id)
                .ToList();

            var categoriesFromDb = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            newModel.Categories = categoriesFromDb;

            _context.News.Add(newModel);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al crear la noticia.");

            return await _context.News
                .Include(d => d.Status)
                .Include(d => d.Categories)
                .FirstAsync(n => n.Id == newModel.Id);
        }
    }
}

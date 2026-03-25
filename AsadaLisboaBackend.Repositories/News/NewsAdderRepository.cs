using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.News;

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
            _context.News.Add(newModel);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new CreateObjectException("Error al crear la noticia.");

            return newModel;
        }
    }
}

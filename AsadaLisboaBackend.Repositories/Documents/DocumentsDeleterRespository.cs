using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.Models.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.Repositories.Documents
{
    public class DocumentsDeleterRespository : IDocumentDeleterRespository
    {
        private readonly ApplicationDbContext _context;

        public DocumentsDeleterRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteDocument(Guid id)
        {
            var affectedRows = await _context.Documents
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();

            if (affectedRows < 1)
                throw new NotFoundException("Error al eliminar el documento")
        }
    }
}

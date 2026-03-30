using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Repositories.Documents
{
    public class DocumentsAdderRepository : IDocumentsAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentsAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Document> CreateDocument(Document newDocument)
        {
            _context.Documents.Add(newDocument);

            var affectedRow = await _context.SaveChangesAsync();

            if (affectedRow < 1)
                throw new CreateObjectException("Error al agrgrar el documento");

            return newDocument;

        }
    }
}

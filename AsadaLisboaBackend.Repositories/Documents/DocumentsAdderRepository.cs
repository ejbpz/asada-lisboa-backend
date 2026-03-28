using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.RepositoryContracts.Documents;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.Repositories.Documents
{
    public class DocumentsAdderRepository : IDocumentAdderRepository
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
                throw new CreateObjectExeption("Error al agrgrar el documento");

            return newDocument;

        }
    }
}

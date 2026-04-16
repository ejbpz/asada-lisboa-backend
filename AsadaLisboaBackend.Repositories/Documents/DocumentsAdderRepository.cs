using Microsoft.EntityFrameworkCore;
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
            var categoryIds = newDocument.Categories
                .Select(c => c.Id)
                .ToList();

            var categoriesFromDb = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            newDocument.Categories = categoriesFromDb;

            _context.Documents.Add(newDocument);
            var affectedRow = await _context.SaveChangesAsync();

            if (affectedRow < 1)
                throw new CreateObjectException("Error al agregrar el documento");

            return await _context.Documents
                .Include(d => d.Status)
                .Include(d => d.Categories)
                .Include(d => d.DocumentType)
                .FirstAsync(d => d.Id == newDocument.Id);
        }
    }
}

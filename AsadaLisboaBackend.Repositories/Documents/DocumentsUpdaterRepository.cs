using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Repositories.Documents
{
    public class DocumentsUpdaterRepository : IDocumentsUpdaterRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentsUpdaterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Document> UpdateDocument(Models.Document document)
        {
            var existingDocument = await _context.Documents
                .Include(d => d.Status)
                .Include(d => d.Categories)
                .Include(d => d.DocumentType)
                .FirstAsync(d => d.Id == document.Id);

            existingDocument.Url = document.Url;
            existingDocument.Slug = document.Slug;
            existingDocument.Title = document.Title;
            existingDocument.StatusId = document.StatusId;
            existingDocument.FileSize = document.FileSize;
            existingDocument.FilePath = document.FilePath;
            existingDocument.FileName = document.FileName;
            existingDocument.Description = document.Description;
            existingDocument.DocumentTypeId = document.DocumentTypeId;

            var newCategories = document.Categories.ToList();
            existingDocument.Categories.Clear();

            foreach (var category in newCategories)
            {
                _context.Categories.Attach(category);
                existingDocument.Categories.Add(category);
            }

            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar el documento");

            return existingDocument;
        }
    }
}

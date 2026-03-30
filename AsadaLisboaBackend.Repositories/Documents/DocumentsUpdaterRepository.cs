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
            _context.Attach(document);

            _context.Entry(document).Property(n => n.Title).IsModified = false;
            _context.Entry(document).Property(n => n.Description).IsModified = false;
            _context.Entry(document).Property(n => n.Slug).IsModified = false;
            _context.Entry(document).Property(n => n.StatusId).IsModified = false;
            _context.Entry(document).Property(n => n.FileSize).IsModified = false;
            _context.Entry(document).Property(n => n.DocumentTypeId).IsModified = false;
            _context.Entry(document).Property(n => n.Categories).IsModified = false;


            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new UpdateObjectException("Error al modificar el documento");

            return new Models.Document()
            {

                Id = document.Id,
                Title = document.Title,
                Description = document.Description,
                Slug = document.Slug,
                Status = document.Status,
                StatusId = document.StatusId,
                FileSize = document.FileSize,
                Categories = document.Categories,
                PublicationDate = document.PublicationDate
            };


        }

    }
}

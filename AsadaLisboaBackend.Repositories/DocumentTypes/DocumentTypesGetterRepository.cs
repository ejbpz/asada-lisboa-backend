using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;

namespace AsadaLisboaBackend.Repositories.DocumentTypes
{
    public class DocumentTypesGetterRepository : IDocumentTypesGetterRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentTypesGetterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Guid? GetDocumentTypeIdByExtension(string extension)
        {
            return _context.DocumentTypes
                .AsNoTracking()
                .Where(c => c.Extension.Equals(extension, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault()?.Id;
        } 
    }
}

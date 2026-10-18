using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.DocumentTypes;

namespace AsadaLisboaBackend.Repositories.DocumentTypes;

public class DocumentTypesGetterRepository(ApplicationDbContext context) : IDocumentTypesGetterRepository
{
    public Guid? GetDocumentTypeIdByExtension(string extension)
    {
        return context.DocumentTypes
            .AsNoTracking()
            .FirstOrDefault(c => c.Extension.Equals(extension))?.Id;
    } 
}
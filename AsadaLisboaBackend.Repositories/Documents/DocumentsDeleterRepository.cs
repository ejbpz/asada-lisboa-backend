using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Documents;

namespace AsadaLisboaBackend.Repositories.Documents;

public class DocumentsDeleterRepository(ApplicationDbContext context) : IDocumentsDeleterRepository
{
    public async Task DeleteDocument(Guid id)
    {
        var affectedRows = await context.Documents
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows < 1)
            throw new NotFoundException("Error al eliminar el documento");
    }
}
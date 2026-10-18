using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts;

public class ContactsDeleterRepository(ApplicationDbContext context) : IContactsDeleterRepository
{
    public async Task DeleteContact(Guid id)
    {
        var affectedRows = await context.Contacts
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows < 1)
            throw new NotFoundException("Error al eliminar el contacto.");
    }
}
using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts;

public class ContactsAdderRepository(ApplicationDbContext context) : IContactsAdderRepository
{
    public async Task<Contact> CreateContact(Contact contact)
    {
        context.Contacts.Add(contact);
        var affectedRows = await context.SaveChangesAsync();

        return affectedRows < 1 ? throw new NotFoundException("Error al crear el contacto.") : contact;
    }
}
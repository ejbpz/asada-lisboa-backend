using AsadaLisboaBackend.Models;
using AsadaLisboaBackend.Models.DatabaseContext;
using AsadaLisboaBackend.RepositoryContracts.Contacts;

namespace AsadaLisboaBackend.Repositories.Contacts
{
    public class ContactsAdderRepository : IContactsAdderRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactsAdderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Contact> CreateContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            var affectedRows = await _context.SaveChangesAsync();

            if (affectedRows < 1)
                throw new Exception("Error al crear el contacto."); // TODO: Create custom exception.

            return contact;
        }
    }
}

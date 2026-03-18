using AsadaLisboaBackend.Models;

namespace AsadaLisboaBackend.RepositoryContracts.Contacts
{
    public interface IContactsAdderRepository
    {
        public Task<Contact> CreateContact(Contact contact);
    }
}

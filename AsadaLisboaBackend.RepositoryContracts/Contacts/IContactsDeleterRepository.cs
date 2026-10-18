namespace AsadaLisboaBackend.RepositoryContracts.Contacts;

public interface IContactsDeleterRepository
{
    public Task DeleteContact(Guid id);
}
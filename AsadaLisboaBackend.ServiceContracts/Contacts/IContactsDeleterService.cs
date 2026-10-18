namespace AsadaLisboaBackend.ServiceContracts.Contacts;

public interface IContactsDeleterService
{
    public Task DeleteContact(Guid id);
}
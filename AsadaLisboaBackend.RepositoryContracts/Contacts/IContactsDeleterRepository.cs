namespace AsadaLisboaBackend.RepositoryContracts.Contacts
{
    public interface IContactsDeleterRepository
    {
        public Task UpdateContact(Guid id);
    }
}

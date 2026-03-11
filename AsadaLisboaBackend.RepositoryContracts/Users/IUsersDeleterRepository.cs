namespace AsadaLisboaBackend.RepositoryContracts.Users
{
    public interface IUsersDeleterRepository
    {
        public Task DeleteUser(Guid id);
    }
}

namespace AsadaLisboaBackend.ServiceContracts.Users
{
    public interface IUsersDeleterService
    {
        public Task DeleteUser(Guid id);
    }
}

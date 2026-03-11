using AsadaLisboaBackend.Models.IdentityModels;

namespace AsadaLisboaBackend.RepositoryContracts.Users
{
    public interface IUsersUpdaterRepository
    {
        public Task UpdateUser(ApplicationUser updateUser);
    }
}

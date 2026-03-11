using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersDeleterService : IUsersDeleterService
    {
        private readonly IUsersGetterService _usersGetterService;
        private readonly IUsersDeleterRepository _usersDeleterRepository;

        public UsersDeleterService(IUsersGetterService usersGetterService, IUsersDeleterRepository usersDeleterRepository)
        {
            _usersGetterService = usersGetterService;
            _usersDeleterRepository = usersDeleterRepository;
        }

        public async Task DeleteUser(Guid id)
        {
            await _usersDeleterRepository.DeleteUser(id);
        }
    }
}

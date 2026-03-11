using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Users;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersGetterService : IUsersGetterService
    {
        private readonly IUsersGetterRepository _usersGetterRepository;

        public UsersGetterService(IUsersGetterRepository usersGetterRepository)
        {
            _usersGetterRepository = usersGetterRepository;
        }

        public async Task<List<UserResponseDTO>?> GetUsers(int page)
        {
            int offset = (Math.Max(page, 1) - 1) * Constants.PAGINATION_SIZE;

            return await _usersGetterRepository.GetUsers(offset, Constants.PAGINATION_SIZE);
        }

        public async Task<UserDetailResponseDTO?> GetUser(Guid id)
        {
            return await _usersGetterRepository.GetUser(id);
        }

        public async Task<bool> UserExists(Guid id)
        {
            return await _usersGetterRepository.UserExists(id);
        }
    }
}

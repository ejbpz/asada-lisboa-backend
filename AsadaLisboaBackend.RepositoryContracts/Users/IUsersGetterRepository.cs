using AsadaLisboaBackend.Models.DTOs.Users;

namespace AsadaLisboaBackend.RepositoryContracts.Users
{
    public interface IUsersGetterRepository
    {
        public Task<List<UserResponseDTO>?> GetUsers(int offset, int take);
        public Task<UserDetailResponseDTO?> GetUser(Guid id);
    }
}

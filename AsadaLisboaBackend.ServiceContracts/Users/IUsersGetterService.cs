using AsadaLisboaBackend.Models.DTOs.Users;

namespace AsadaLisboaBackend.ServiceContracts.Users
{
    public interface IUsersGetterService
    {
        public Task<List<UserResponseDTO>?> GetUsers(int page);
        public Task<UserDetailResponseDTO?> GetUser(Guid id);
    }
}

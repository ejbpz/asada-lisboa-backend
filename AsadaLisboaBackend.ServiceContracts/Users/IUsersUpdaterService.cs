using AsadaLisboaBackend.Models.DTOs.Users;

namespace AsadaLisboaBackend.ServiceContracts.Users
{
    public interface IUsersUpdaterService
    {
        public Task UpdateUser(Guid id, UserUpdateRequestDTO userUpdateRequestDTO);
    }
}

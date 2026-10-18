using AsadaLisboaBackend.Models.DTOs.User;

namespace AsadaLisboaBackend.ServiceContracts.Users;

public interface IUsersUpdaterService
{
    public Task UpdateUser(Guid id, UserUpdateRequestDto userUpdateRequestDto);
}
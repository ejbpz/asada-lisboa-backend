using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.ServiceContracts.Users;

public interface IUsersGetterService
{
    public Task<PageResponseDto<UserResponseDto>> GetUsers(SearchSortRequestDto searchSortRequestDto);
    public Task<UserDetailResponseDto?> GetUser(Guid id);
}
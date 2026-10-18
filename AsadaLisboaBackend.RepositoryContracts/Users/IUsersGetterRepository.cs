using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.Users;

public interface IUsersGetterRepository
{
    public Task<PageResponseDto<UserResponseDto>> GetUsers(SearchSortRequestDto searchSortRequestDto);
    public Task<UserDetailResponseDto?> GetUser(Guid id);
}
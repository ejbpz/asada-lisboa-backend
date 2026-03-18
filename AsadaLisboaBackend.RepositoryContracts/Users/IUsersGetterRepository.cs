using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;

namespace AsadaLisboaBackend.RepositoryContracts.Users
{
    public interface IUsersGetterRepository
    {
        public Task<PageResponseDTO<UserResponseDTO>> GetUsers(SearchSortRequestDTO searchSortRequestDTO);
        public Task<UserDetailResponseDTO?> GetUser(Guid id);
    }
}

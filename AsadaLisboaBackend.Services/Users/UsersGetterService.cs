using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
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

        public async Task<PageResponseDTO<UserResponseDTO>> GetUsers(SearchSortRequestDTO searchSortRequestDTO)
        {
            searchSortRequestDTO.Offset = (Math.Max(searchSortRequestDTO.Page, 1) - 1) * searchSortRequestDTO.Take;

            return await _usersGetterRepository.GetUsers(searchSortRequestDTO);
        }

        public async Task<UserDetailResponseDTO?> GetUser(Guid id)
        {
            var user = await _usersGetterRepository.GetUser(id);

            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            return user;
        }
    }
}

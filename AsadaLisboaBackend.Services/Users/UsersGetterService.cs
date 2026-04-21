using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersGetterService : IUsersGetterService
    {
        private readonly ILogger<UsersGetterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly IUsersGetterRepository _usersGetterRepository;

        public UsersGetterService(IUsersGetterRepository usersGetterRepository, ILogger<UsersGetterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _memoryCachesService = memoryCachesService;
            _usersGetterRepository = usersGetterRepository;
        }

        public async Task<PageResponseDTO<UserResponseDTO>> GetUsers(SearchSortRequestDTO searchSortRequestDTO)
        {
            return await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<UserResponseDTO>>(
                resource: Constants.CACHE_USERS,
                request: searchSortRequestDTO,
                create: () => _usersGetterRepository.GetUsers(searchSortRequestDTO),
                time: TimeSpan.FromMinutes(5));
        }

        public async Task<PageResponseDTO<UserResponseDTO>> GetPublicUsers(SearchSortRequestDTO searchSortRequestDTO)
        {
            var usersList = await _memoryCachesService.GetOrCreateCacheList<PageResponseDTO<UserResponseDTO>>(
                resource: Constants.CACHE_USERS,
                request: searchSortRequestDTO,
                create: () => _usersGetterRepository.GetUsers(searchSortRequestDTO),
                time: TimeSpan.FromMinutes(5));

            usersList.Data = usersList.Data.Where(u => u.Charge != "Administrador")
                .ToList();

            return usersList;
        }

        public async Task<UserDetailResponseDTO?> GetUser(Guid id)
        {
            var user = await _memoryCachesService.GetOrCreateCache<UserDetailResponseDTO?>(
                key: $"{Constants.CACHE_USERS}_{id}",
                create: () => _usersGetterRepository.GetUser(id),
                time: TimeSpan.FromMinutes(5));

            if (user is null)
            {
                _logger.LogWarning("Usuario con id {UserId} no encontrado.", id);
                throw new NotFoundException("Usuario inexistente.");
            }

            return user;
        }
    }
}

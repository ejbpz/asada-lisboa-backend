using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Shared;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Users;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Users;

public class UsersGetterService(
    IUsersGetterRepository usersGetterRepository, 
    ILogger<UsersGetterService> logger, 
    IMemoryCachesService memoryCachesService) : IUsersGetterService
{
    public async Task<PageResponseDto<UserResponseDto>> GetUsers(SearchSortRequestDto searchSortRequestDto)
    {
        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_USERS,
            request: searchSortRequestDto,
            create: () => usersGetterRepository.GetUsers(searchSortRequestDto),
            time: TimeSpan.FromMinutes(5));
    }

    public async Task<UserDetailResponseDto?> GetUser(Guid id)
    {
        var user = await memoryCachesService.GetOrCreateCache(
            key: $"{Constants.CACHE_USERS}_{id}",
            create: () => usersGetterRepository.GetUser(id),
            time: TimeSpan.FromMinutes(5));

        if (user is null)
        {
            logger.LogWarning("Usuario con id {UserId} no encontrado.", id);
            throw new NotFoundException("Usuario inexistente.");
        }

        return user;
    }
}
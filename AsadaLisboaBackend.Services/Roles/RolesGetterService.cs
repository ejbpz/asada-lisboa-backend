using Microsoft.Extensions.Logging;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Role;
using AsadaLisboaBackend.ServiceContracts.Roles;
using AsadaLisboaBackend.RepositoryContracts.Roles;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Roles;

public class RolesGetterService(
    IRolesGetterRepository rolesGetterRepository,
    ILogger<RolesGetterService> logger,
    IMemoryCachesService memoryCachesService)
    : IRolesGetterService
{
    public async Task<List<RoleResponseDto>> GetRoles()
    {
        logger.LogInformation("Los roles para usuarios han sido solicitados.");

        return await memoryCachesService.GetOrCreateCacheList(
            resource: Constants.CACHE_ROLES,
            request: "",
            create: rolesGetterRepository.GetRoles,
            time: TimeSpan.FromMinutes(5));
    }
}
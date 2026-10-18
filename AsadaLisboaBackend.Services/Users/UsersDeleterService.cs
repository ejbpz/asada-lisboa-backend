using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Users;

public class UsersDeleterService(
    UserManager<ApplicationUser> userManager,
    ILogger<UsersDeleterService> logger,
    IMemoryCachesService memoryCachesService)
    : IUsersDeleterService
{
    public async Task DeleteUser(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            logger.LogWarning("Usuario con id {UserId} no encontrado para eliminación.", id);
            throw new NotFoundException("Usuario inexistente.");
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error al eliminar el usuario con id {UserId}.", id);
            throw new InvalidOperationException("Error al eliminar el usuario.");
        }

        memoryCachesService.RemoveById(Constants.CACHE_USERS, user.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_USERS);

        logger.LogInformation("Usuario con id {UserId} eliminado exitosamente.", id);
    }
}
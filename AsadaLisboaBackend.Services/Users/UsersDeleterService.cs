using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Users
{
    public class UsersDeleterService : IUsersDeleterService
    {
        private readonly ILogger<UsersDeleterService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersDeleterService(UserManager<ApplicationUser> userManager, ILogger<UsersDeleterService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _userManager = userManager;
            _memoryCachesService = memoryCachesService;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                _logger.LogWarning("Usuario con id {UserId} no encontrado para eliminación.", id);
                throw new NotFoundException("Usuario inexistente.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Error al eliminar el usuario con id {UserId}.", id);
                throw new InvalidOperationException("Error al eliminar el usuario.");
            }

            _memoryCachesService.RemoveById(Constants.CACHE_USERS, user.Id);
            _memoryCachesService.ChangeVersion(Constants.CACHE_USERS);

            _logger.LogInformation("Usuario con id {UserId} eliminado exitosamente.", id);
        }
    }
}

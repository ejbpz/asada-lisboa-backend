using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.User;
using AsadaLisboaBackend.Models.DTOs.Error;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Users;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Users;

public class UsersUpdaterService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IChargesGetterRepository chargesGetterRepository,
    ILogger<UsersUpdaterService> logger,
    IMemoryCachesService memoryCachesService)
    : IUsersUpdaterService
{
    public async Task UpdateUser(Guid id, UserUpdateRequestDto userUpdateRequestDto)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            logger.LogError("Intento de actualización de usuario con id {UserId} no encontrado.", id);
            throw new NotFoundException("Usuario inexistente.");
        }

        var charge = await chargesGetterRepository.GetCharge(userUpdateRequestDto.ChargeId);
        var role = await roleManager.FindByIdAsync(userUpdateRequestDto.RoleId.ToString());

        if (role is null)
        {
            logger.LogError("Rol seleccionado con id {RoleId} no encontrado para el usuario {UserId}.", userUpdateRequestDto.RoleId, id);
            throw new NotFoundException("Rol seleccionado no encontrado.");
        }

        user.ChargeId = charge.Id;
        user.FirstName = userUpdateRequestDto.FirstName;
        user.PhoneNumber = userUpdateRequestDto.PhoneNumber;
        user.FirstLastName = userUpdateRequestDto.FirstLastName;
        user.SecondLastName = userUpdateRequestDto.SecondLastName;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error al actualizar usuario con id {UserId}. Errores: {Errors}", id, string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));
            throw new IdentityErrorException(
                "Error al actualizar usuario.", 
                result.Errors.Select(e => new ErrorDetailResponseDto(e.Code, e.Description)
                ).ToList()
            );
        }

        var currentRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();

        if(!string.IsNullOrEmpty(currentRole) && !string.IsNullOrWhiteSpace(currentRole))
        {
            if (!role.Name!.Equals(currentRole, StringComparison.InvariantCultureIgnoreCase))
                await userManager.RemoveFromRoleAsync(user, currentRole);
        }

        await userManager.AddToRoleAsync(user, role.Name!);

        memoryCachesService.RemoveById(Constants.CACHE_USERS, user.Id);
        memoryCachesService.ChangeVersion(Constants.CACHE_USERS);
    }
}
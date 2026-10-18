using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Accounts;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Accounts;

public class RegisterUserService(
    UserManager<ApplicationUser> userManager, 
    RoleManager<ApplicationRole> roleManager, 
    IVerificationCodeService verificationCodeService, 
    IChargesGetterRepository chargesGetterRepository, 
    ILogger<RegisterUserService> logger, 
    IMemoryCachesService memoryCachesService) 
    : IRegisterUserService
{
    public async Task RegisterUser(RegisterRequestDto registerRequestDto)
    {
        //Verify if email exists
        var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Email);

        if (existingUser != null)
        {
            logger.LogError("Error al registrar usuario: Correo electrónico {Email} en uso.", registerRequestDto.Email);
            throw new NotFoundException("El correo electrónico ya esta registrado.");
        }

        var charge = await chargesGetterRepository.GetCharge(registerRequestDto.ChargeId);
        var role = await roleManager.FindByIdAsync(registerRequestDto.RoleId.ToString());

        if(role is null)
        {
            logger.LogError("Rol seleccionado con id {RoleId} no encontrado para el usuario.", registerRequestDto.RoleId);
            throw new NotFoundException("Rol seleccionado no encontrado.");
        }

        //Register new user
        var user = new ApplicationUser
        {
            ChargeId = charge.Id,
            Email = registerRequestDto.Email,
            UserName = registerRequestDto.Email,
            FirstName = registerRequestDto.FirstName,
            PhoneNumber = registerRequestDto.PhoneNumber,
            FirstLastName = registerRequestDto.FirstLastName,
            SecondLastName = registerRequestDto.SecondLastName,
        };

        var userResult = await userManager.CreateAsync(user, registerRequestDto.Password);

        if (!userResult.Succeeded)
        {
            logger.LogError("Error al registrar usuario: {Errors}", string.Join(", ", userResult.Errors.Select(e => e.Description)));
            throw new RegisterUserException("Error al registrar usuario.");
        }

        var roleResult = await userManager.AddToRoleAsync(user, role.Name!);

        if (!roleResult.Succeeded)
        {
            logger.LogError("Error al asignar rol al usuario: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            throw new RegisterUserException("Error al asignar rol al usuario.");
        }

        await verificationCodeService.GenerateCode(user.Email);

        memoryCachesService.ChangeVersion(Constants.CACHE_USERS);
    }
}
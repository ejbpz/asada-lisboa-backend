using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Models.DTOs.Error;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Services.Accounts;

public class ResetPasswordService(
    UserManager<ApplicationUser> userManager,
    IEmailsSenderService emailsSenderService,
    ILogger<ResetPasswordService> logger)
    : IResetPasswordService
{
    public async Task ForgotPassword(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !user.EmailConfirmed)
        {
            logger.LogError("Intento de recuperación de contraseña para un correo electrónico no registrado o no confirmado: {Email}", email);
            throw new NotFoundException("No existe un usuario con este correo electrónico.");
        }

        var resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await userManager.GeneratePasswordResetTokenAsync(user)));
        await emailsSenderService.SendResetPasswordToken(user.FirstName, email, resetToken);
    }

    public async Task ResetPassword(string email, string token, string password)
    {
        token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !user.EmailConfirmed)
        {
            logger.LogError("Intento de restauración de contraseña para un correo electrónico no registrado o no confirmado: {Email}", email);
            throw new NotFoundException("No existe un usuario con este correo electrónico.");
        }

        var result = await userManager.ResetPasswordAsync(user, token, password);

        if (!result.Succeeded)
        {
            logger.LogError("Error al restaurar contraseña para el correo electrónico {Email}. Errores: {Errors}", email, string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));
            throw new IdentityErrorException(
                "Error al restaurar contraseña.",
                result.Errors.Select(e => new ErrorDetailResponseDto(e.Code, e.Description)
                ).ToList()
            );
        }
    }
}
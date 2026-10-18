using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Services.Accounts;

public class VerificationCodeService(
    UserManager<ApplicationUser> userManager,
    IEmailsSenderService emailsSenderService,
    ILogger<VerificationCodeService> logger)
    : IVerificationCodeService
{
    public async Task GenerateCode(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            logger.LogError("Usuario no existente para generación de token de verificación.");
            throw new NotFoundException("Usuario inexistente.");
        }

        var generateToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(generateToken));

        await emailsSenderService.SendVerificationCode(user.FirstName, email, encodedToken);
    }

    public async Task ConfirmEmailAsync(string? email, string? token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            logger.LogError("Email o token de verificación no proporcionado.");
            throw new ArgumentException("Email y token son requeridos.");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            logger.LogError("Usuario no existente para confirmación de email.");
            throw new NotFoundException("Usuario inexistente.");
        }

        if(user.EmailConfirmed)
        {
            logger.LogError("El email del usuario ya ha sido confirmado.");
            throw new UpdateObjectException("El email del usuario ya ha sido confirmado.");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            logger.LogError("Error al confirmar el email del usuario.");
            throw new UpdateObjectException("Error al actualizar el usuario.");
        }

        user.EmailConfirmed = true;
        user.IsActive = true;

        await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error al confirmar el email del usuario.");
            throw new UpdateObjectException("Error al actualizar el usuario.");
        }
    }
}
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Services.Accounts
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly ILogger<VerificationCodeService> _logger;
        private readonly IEmailsSenderService _emailsSenderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerificationCodeService(UserManager<ApplicationUser> userManager, IEmailsSenderService emailsSenderService, ILogger<VerificationCodeService> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _emailsSenderService = emailsSenderService;
        }

        public async Task GenerateCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                _logger.LogError("Usuario no existente para generación de token de verificación.");
                throw new NotFoundException("Usuario inexistente.");
            }

            var generateToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(generateToken));

            await _emailsSenderService.SendVerificationCode(user.FirstName, email, encodedToken);
        }

        public async Task ConfirmEmailAsync(string? email, string? token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                _logger.LogError("Email o token de verificación no proporcionado.");
                throw new ArgumentException("Email y token son requeridos.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                _logger.LogError("Usuario no existente para confirmación de email.");
                throw new NotFoundException("Usuario inexistente.");
            }

            if(user.EmailConfirmed)
            {
                _logger.LogError("El email del usuario ya ha sido confirmado.");
                throw new UpdateObjectException("El email del usuario ya ha sido confirmado.");
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            user.EmailConfirmed = true;
            user.IsActive = true;
            
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Error al confirmar el email del usuario.");
                throw new UpdateObjectException("Error al actualizar el usuario.");
            }
        }
    }
}

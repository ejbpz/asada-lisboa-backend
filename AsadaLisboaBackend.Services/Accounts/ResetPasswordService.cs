using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Models.DTOs.Error;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Services.Accounts
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IEmailsSenderService _emailsSenderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordService(UserManager<ApplicationUser> userManager, IEmailsSenderService emailsSenderService)
        {
            _emailsSenderService = emailsSenderService;
            _userManager = userManager; 
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !user.EmailConfirmed)
                throw new NotFoundException("No existe un usuario con este correo electrónico.");

            string resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user)));

            await _emailsSenderService.SendResetPasswordToken(user.FirstName, email, resetToken);
        }

        public async Task ResetPassword(string email, string token, string password)
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !user.EmailConfirmed) 
                throw new NotFoundException("No existe un usuario con este correo electrónico.");

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
                throw new IdentityErrorException(
                    "Error al restaurar contraseña.",
                    result.Errors.Select(e => new ErrorDetailResponseDTO(e.Code, e.Description)
                    ).ToList()
                );
        }
    }
}

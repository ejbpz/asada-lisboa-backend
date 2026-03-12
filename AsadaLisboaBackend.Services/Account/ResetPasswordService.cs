using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Services.Account
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordService(UserManager<ApplicationUser> userManager, IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
            _userManager = userManager; 
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !user.EmailConfirmed) 
                throw new ArgumentNullException("No existe un usuario con este correo electrónico.");

            string resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await _userManager.GeneratePasswordResetTokenAsync(user)));

            return await _emailSenderService.SendResetPasswordToken(user.FirstName, email, resetToken);
        }

        public async Task ResetPassword(string email, string token, string password)
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) 
                throw new ArgumentNullException("No existe un usuario con este correo electrónico.");

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            // TODO: Add errors.
            if (!result.Succeeded)
                throw new ArgumentNullException("Error al restaurar contraseña.");
        }
    }
}

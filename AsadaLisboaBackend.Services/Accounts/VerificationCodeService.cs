using System.Text;
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
        private readonly IEmailsSenderService _emailsSenderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerificationCodeService(UserManager<ApplicationUser> userManager, IEmailsSenderService emailsSenderService)
        {
            _userManager = userManager;
            _emailsSenderService = emailsSenderService;
        }

        public async Task GenerateCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            var generateToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(generateToken));

            await _emailsSenderService.SendVerificationCode(user.FirstName, email, encodedToken);
        }

        public async Task ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new NotFoundException("Usuario inexistente.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            user.EmailConfirmed = true;
            user.IsActive = true;
            
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new UpdateObjectException("Error al actualizar el usuario.");
        }
    }
}

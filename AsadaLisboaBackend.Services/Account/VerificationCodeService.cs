using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Services.Account
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationCodeSendService _verificationCodeSendService;

        public VerificationCodeService(UserManager<ApplicationUser> userManager, IVerificationCodeSendService verificationCodeSendService)
        {
            _userManager = userManager;
            _verificationCodeSendService = verificationCodeSendService;
        }

        public async Task<bool> GenerateCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return false;

            var generateToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(generateToken));

            //Send email with code 
            return await _verificationCodeSendService.SendVerificationCode(user.FirstName, email, encodedToken);
        }

        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return false;

            // Decodificar token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            user.EmailConfirmed = true;
            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            return result.Succeeded;

        }
    }
}

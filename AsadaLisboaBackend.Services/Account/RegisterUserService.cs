using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Services.Account
{
    public class RegisterUserService : IRegisterUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerificationCodeService _verificationCodeService;

        public RegisterUserService(UserManager<ApplicationUser> userManager, IVerificationCodeService verificationCodeService)
        {
            _userManager = userManager;
            _verificationCodeService = verificationCodeService;
        }

        public async Task RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            //Verify if email exists
            var existinguser = await _userManager.FindByEmailAsync(registerRequestDTO.Email);

            if (existinguser != null)
                throw new NotFoundException("El correo electrónico ya esta registrado.");

            //Register new user
            var user = new ApplicationUser
            {

                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

            if (!result.Succeeded)
                throw new Exception("Error al registrar usuario.");

            var emailSent = await _verificationCodeService.GenerateCode(user.Email);

            if (!emailSent)
                throw new Exception("Error al enviar el correo de confirmación.");
        }
    }
}

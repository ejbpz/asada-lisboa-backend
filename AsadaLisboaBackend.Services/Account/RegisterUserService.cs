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
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDTO.Email);

            if (existingUser != null)
                throw new NotFoundException("El correo electrónico ya esta registrado.");

            //Register new user
            var user = new ApplicationUser
            {
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email,
                ChargeId = registerRequestDTO.ChargeId,
                FirstName = registerRequestDTO.FirstName,
                PhoneNumber = registerRequestDTO.PhoneNumber,
                FirstLastName = registerRequestDTO.FirstLastName,
                SecondLastName = registerRequestDTO.SecondLastName,
            };

            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);

            if (!result.Succeeded)
                throw new RegisterUserException("Error al registrar usuario.");

            await _verificationCodeService.GenerateCode(user.Email);
        }
    }
}

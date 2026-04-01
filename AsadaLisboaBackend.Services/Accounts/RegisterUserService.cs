using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Accounts;
using AsadaLisboaBackend.RepositoryContracts.Charges;

namespace AsadaLisboaBackend.Services.Accounts
{
    public class RegisterUserService : IRegisterUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IChargesGetterRepository _chargesGetterRepository;
        private readonly IVerificationCodeService _verificationCodeService;

        public RegisterUserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IVerificationCodeService verificationCodeService, IChargesGetterRepository chargesGetterRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _chargesGetterRepository = chargesGetterRepository;
            _verificationCodeService = verificationCodeService;
        }

        public async Task RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            //Verify if email exists
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDTO.Email);

            if (existingUser != null)
                throw new NotFoundException("El correo electrónico ya esta registrado.");

            var charge = await _chargesGetterRepository.GetCharge(registerRequestDTO.ChargeId);
            
            if (charge is null)
                throw new NotFoundException("Cargo seleccionado no encontrado.");

            var role = await _roleManager.FindByIdAsync(registerRequestDTO.RoleId.ToString());

            if(role is null)
                throw new NotFoundException("Rol seleccionado no encontrado.");

            //Register new user
            var user = new ApplicationUser
            {
                ChargeId = charge.Id,
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email,
                FirstName = registerRequestDTO.FirstName,
                PhoneNumber = registerRequestDTO.PhoneNumber,
                FirstLastName = registerRequestDTO.FirstLastName,
                SecondLastName = registerRequestDTO.SecondLastName,
            };

            var userResult = await _userManager.CreateAsync(user, registerRequestDTO.Password);

            if (!userResult.Succeeded)
                throw new RegisterUserException("Error al registrar usuario.");

            var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!roleResult.Succeeded)
                throw new RegisterUserException("Error al asignar rol al usuario.");

            await _verificationCodeService.GenerateCode(user.Email);
        }
    }
}

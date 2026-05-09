using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Accounts;
using AsadaLisboaBackend.RepositoryContracts.Charges;
using AsadaLisboaBackend.ServiceContracts.MemoryCaches;

namespace AsadaLisboaBackend.Services.Accounts
{
    public class RegisterUserService : IRegisterUserService
    {

        private readonly ILogger<RegisterUserService> _logger;
        private readonly IMemoryCachesService _memoryCachesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IChargesGetterRepository _chargesGetterRepository;
        private readonly IVerificationCodeService _verificationCodeService;

        public RegisterUserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IVerificationCodeService verificationCodeService, IChargesGetterRepository chargesGetterRepository, ILogger<RegisterUserService> logger, IMemoryCachesService memoryCachesService)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _memoryCachesService = memoryCachesService;
            _chargesGetterRepository = chargesGetterRepository;
            _verificationCodeService = verificationCodeService;
        }

        public async Task RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            //Verify if email exists
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDTO.Email);

            if (existingUser != null)
            {
                _logger.LogError("Error al registrar usuario: Correo electrónico {Email} en uso.", registerRequestDTO.Email);
                throw new NotFoundException("El correo electrónico ya esta registrado.");
            }

            var charge = await _chargesGetterRepository.GetCharge(registerRequestDTO.ChargeId);
            
            if (charge is null)
            {
                _logger.LogError("Cargo seleccionado con id {ChargeId} no encontrado para el usuario.", registerRequestDTO.ChargeId);
                throw new NotFoundException("Cargo seleccionado no encontrado.");
            }

            var role = await _roleManager.FindByIdAsync(registerRequestDTO.RoleId.ToString());

            if(role is null)
            {
                _logger.LogError("Rol seleccionado con id {RoleId} no encontrado para el usuario.", registerRequestDTO.RoleId);
                throw new NotFoundException("Rol seleccionado no encontrado.");
            }

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
            {
                _logger.LogError("Error al registrar usuario: {Errors}", string.Join(", ", userResult.Errors.Select(e => e.Description)));
                throw new RegisterUserException("Error al registrar usuario.");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!roleResult.Succeeded)
            {
                _logger.LogError("Error al asignar rol al usuario: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                throw new RegisterUserException("Error al asignar rol al usuario.");
            }

            await _verificationCodeService.GenerateCode(user.Email);

            _memoryCachesService.ChangeVersion(Constants.CACHE_USERS);
        }
    }
}

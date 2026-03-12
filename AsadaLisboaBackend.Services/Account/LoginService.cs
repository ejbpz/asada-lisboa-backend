using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Services.Account
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user is null)
                throw new ArgumentNullException("No existe un usuario con este correo electrónico.");

            var result = await _signInManager.PasswordSignInAsync(user, loginRequestDTO.Password, isPersistent: true, lockoutOnFailure: false);

            // TODO: Add errors.
            if (!result.Succeeded)
                throw new ArgumentException("Correo electrónico y/o contraseña incorrectos.");
        }
    }
}

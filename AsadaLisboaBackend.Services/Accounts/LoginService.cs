using Microsoft.AspNetCore.Identity;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.Services.Exceptions;
using AsadaLisboaBackend.ServiceContracts.Jwts;
using AsadaLisboaBackend.Models.IdentityModels;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Services.Accounts
{
    public class LoginService : ILoginService
    {
        private readonly IJwtsService _jwtsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtsService jwtsService)
        {
            _jwtsService = jwtsService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

            if (user is null)
                throw new NotFoundException("No existe un usuario con este correo electrónico.");

            var result = await _signInManager.PasswordSignInAsync(user, loginRequestDTO.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new InvalidCredentialsException("Correo electrónico y/o contraseña incorrectos.");

            var autenticationResponse = _jwtsService.GenerateToken(user);

            user.RefreshToken = autenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = autenticationResponse.RefreshTokenExpiration;

            await _userManager.UpdateAsync(user);

            return autenticationResponse;
        }
    }
}

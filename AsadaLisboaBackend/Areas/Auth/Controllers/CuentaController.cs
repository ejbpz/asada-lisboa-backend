using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Utils;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Jwts;
using AsadaLisboaBackend.ServiceContracts.Accounts;

namespace AsadaLisboaBackend.Areas.Auth.Controllers
{
    /// <summary>
    /// Controller for manage the user account.
    /// </summary>
    [ApiController]
    [Area("Auth")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly IJwtsService _jwtsService;
        private readonly ILoginService _loginService;
        private readonly IResetPasswordService _resetPasswordService;

        /// <summary>
        /// Constructor for CuentaController.
        /// </summary>
        /// <param name="loginService">Service for login users.</param>
        /// <param name="jwtsService">Service for managing tokens.</param>
        /// <param name="resetPasswordService">Service for reset passwords.</param>
        public CuentaController(IResetPasswordService resetPasswordService, ILoginService loginService, IJwtsService jwtsService)
        {
            _jwtsService = jwtsService;
            _loginService = loginService;
            _resetPasswordService = resetPasswordService;
        }

        /// <summary>
        /// Log in an user based on the provided LoginRequestDTO object.
        /// </summary>
        /// <param name="loginRequestDTO">An object containing the details of the login requests (email + password).</param>
        /// <returns>An ActionResult containing the logged in object.</returns>
        [HttpPost("iniciar-sesion")]
        public async Task<ActionResult<AuthenticationResponseDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            return Ok(await _loginService.Login(loginRequestDTO));
        }

        /// <summary>
        /// Log out an user by its token sent in the Authorization header.
        /// </summary>
        /// <returns>No content.</returns>
        [Authorize(Policy = Constants.ROLE_LECTOR)]
        [HttpPost("cerrar-sesion")]
        public async Task<IActionResult> Logout()
        {
            await _jwtsService.DeleteToken();
            return NoContent();
        }

        /// <summary>
        /// Refresh the user token.
        /// </summary>
        /// <param name="refreshTokenRequestDTO">An object containing the details of the refresh token.</param>
        /// <returns>An ActionResult containing the created token and refresh token object.</returns>
        [HttpPost("refrescar-token")]
        public async Task<ActionResult<AuthenticationResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            return Ok(await _jwtsService.ValidateRefreshToken(refreshTokenRequestDTO));
        }

        /// <summary>
        /// Request token to change password.
        /// </summary>
        /// <param name="resetPasswordDTO">An object containing the email of the user.</param>
        /// <returns>No content.</returns>
        [HttpPost("olvidar-contrasena")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO resetPasswordDTO)
        {
            await _resetPasswordService.ForgotPassword(resetPasswordDTO.Email);
            return NoContent();
        }

        /// <summary>
        /// Reset the password with token given by email.
        /// </summary>
        /// <param name="resetPasswordRequestDTO">An object containing the details of the new password (email, token, password and confirmPassword).</param>
        /// <returns>No content.</returns>
        [HttpPost("restaurar-contrasena")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            await _resetPasswordService.ResetPassword(resetPasswordRequestDTO.Email, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.Password);
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Jwt;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Jwt;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILoginService _loginService;
        private readonly IResetPasswordService _resetPasswordService;

        public CuentaController(IResetPasswordService resetPasswordService, ILoginService loginService, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _loginService = loginService;
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost("iniciar-sesion")]
        public async Task<ActionResult<AuthenticationResponseDTO>> Login([FromForm] LoginRequestDTO loginRequestDTO)
        {
            return Ok(await _loginService.Login(loginRequestDTO));
        }

        [Authorize]
        [HttpPost("cerrar-sesion")]
        public async Task<IActionResult> Logout()
        {
            await _jwtService.DeleteToken();
            return Ok();
        } 

        [HttpPost("refrescar-token")]
        public ActionResult<AuthenticationResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            return Ok(_jwtService.ValidateRefreshToken(refreshTokenRequestDTO));
        }

        [HttpPost("olvidar-contrasena")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO resetPasswordDTO)
        {
            await _resetPasswordService.ForgotPassword(resetPasswordDTO.Email);
            return NoContent();
        }

        [HttpPost("restaurar-contrasena")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            await _resetPasswordService.ResetPassword(resetPasswordRequestDTO.Email, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.Password);
            return NoContent();
        }
    }
}
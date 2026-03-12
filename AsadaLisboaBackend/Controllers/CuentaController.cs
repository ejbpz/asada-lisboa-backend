using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IResetPasswordService _resetPasswordService;

        public CuentaController(IResetPasswordService resetPasswordService, ILoginService loginService)
        {
            _loginService = loginService;
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSesion(LoginRequestDTO loginRequestDTO)
        {
            await _loginService.Login(loginRequestDTO);
            return Ok();
        }

        [HttpPost("olvidar-contrasena")]
        public async Task<IActionResult> OlvidarContrasena([FromBody] ForgotPasswordRequestDTO resetPasswordDTO)
        {
            return Ok(await _resetPasswordService.ForgotPassword(resetPasswordDTO.Email));
        }

        [HttpPost("restaurar-contrasena")]
        public async Task<IActionResult> RestaurarContrasena([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            await _resetPasswordService.ResetPassword(resetPasswordRequestDTO.Email, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.Password);
            return NoContent();
        }
    }
}
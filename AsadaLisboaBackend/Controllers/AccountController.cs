using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Models.DTOs;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [Route("api/cuenta")]
    public class AccountController : ControllerBase
    {
        private readonly IResetPasswordService _resetPasswordService;

        public AccountController(IResetPasswordService resetPasswordService)
        {
            _resetPasswordService = resetPasswordService;
        }

        [HttpPost("olvidar-contrasena")]
        public async Task<IActionResult> OlvidarContrasena([FromBody] ForgotPasswordRequestDTO resetPasswordDTO)
        {
            return Ok(await _resetPasswordService.ForgotPassword(resetPasswordDTO.Email));
        }

        [HttpPost("restaurar-contrasena")]
        public async Task<IActionResult> RestaurarContrasena([FromBody] ResetPasswordRequestDTO resetPasswordRequestDTO)
        {
            var result = await _resetPasswordService.ResetPassword(resetPasswordRequestDTO.Email, resetPasswordRequestDTO.Token, resetPasswordRequestDTO.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("password", error.Description);

                return ValidationProblem(ModelState);
            }

            return Ok();
        }
    }
}
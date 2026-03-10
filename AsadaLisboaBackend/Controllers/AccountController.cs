using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs;
using AsadaLisboaBackend.ServiceContracts.ResetPassword;

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
        public async Task<IActionResult> OlvidarContrasena([FromBody] ResetPasswordRequestDTO resetPasswordDTO)
        {
            string email = resetPasswordDTO.Email.Trim();

            var validEmail = Constants.EMAIL_REGEX.Match(email).Success;
            if (!validEmail) throw new ArgumentException("No corresponde a un formato de correo electrónico.");

            return Ok(await _resetPasswordService.ResetPassword(email));
        }
    }
}
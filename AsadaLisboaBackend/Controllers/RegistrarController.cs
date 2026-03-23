using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Account;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class RegistrarController : ControllerBase
    {
        private readonly IRegisterUserService _userService;
        private readonly IVerificationCodeService _verificationCodeService;

        public RegistrarController(IRegisterUserService userService, IVerificationCodeService verificationCodeService)
        {
            _userService = userService;
            _verificationCodeService = verificationCodeService;
        }

        [HttpPost("")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.RegisterUser(dto);

            return Created();
        }

        [HttpPost("confirmar-correo")]
        public async Task<IActionResult> VerifyEmaill([FromQuery] VerificationCodeRequestDTO request)
        {
            var result = await _verificationCodeService.ConfirmEmailAsync(request.Email, request.Token);
            if (!result) return BadRequest("Token inválido o expirado.");
            return Ok("Correo confirmado correctamente.");

        }
    }
}

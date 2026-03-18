using AsadaLisboaBackend.Models.DTOs.Account;
using AsadaLisboaBackend.ServiceContracts.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsadaLisboaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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

            var result = await _userService.RegisterUser(dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Usuario registrado correctamente" });
        }

        [HttpPost("envio-codigo")]
        public async Task<IActionResult> SendVerificationCode([FromBody] VerificationCodeRequestDTO request)
        {
            var result = await _verificationCodeService.GenerateCode(request.Email);
            if (!result) return BadRequest("No se pudo enviar el código.");
            return Ok("Código enviado correctamente.");
        }

        [HttpPost("confirmar-correo")]
        public async Task<IActionResult> VerifyEmaill([FromBody] VerificationCodeRequestDTO request)
        {
            var result = await _verificationCodeService.ConfirmEmailAsync(request.Email, request.Token);
            if (!result) return BadRequest("Token inválido o expirado.");
            return Ok("Correo confirmado correctamente.");

        }
    }
}

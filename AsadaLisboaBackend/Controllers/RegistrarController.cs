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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            await _userService.RegisterUser(registerRequestDTO);
            return Created();
        }

        [HttpPost("confirmar-correo")]
        public async Task<IActionResult> VerifyEmaill([FromQuery] VerificationCodeRequestDTO request)
        {
            await _verificationCodeService.ConfirmEmailAsync(request.Email, request.Token);
            return NoContent();

        }
    }
}

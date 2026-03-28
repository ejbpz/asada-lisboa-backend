using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Email;
using AsadaLisboaBackend.ServiceContracts.ReCaptcha;
using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [EnableRateLimiting("contact-limiter")]
    public class EmailController : ControllerBase
    {
        private readonly ReCaptchaOptions _reCaptchaOptions;
        private readonly IReCaptchaService _reCaptchaService;
        private readonly IEmailSenderService _emailSenderService;

        public EmailController(IEmailSenderService emailSenderService, IOptions<ReCaptchaOptions> options, IReCaptchaService reCaptchaService)
        {
            _reCaptchaOptions = options.Value;
            _reCaptchaService = reCaptchaService;
            _emailSenderService = emailSenderService;
        }

        [HttpGet("re-captcha")]
        public async Task<bool> GetReCaptcha(string reCaptchaResponse)
        {
            if (string.IsNullOrEmpty(reCaptchaResponse) && string.IsNullOrWhiteSpace(reCaptchaResponse))
                throw new ArgumentNullException("El reCaptcha ha sido nulo.");

            if (string.IsNullOrEmpty(_reCaptchaOptions.SECRET_KEY) && string.IsNullOrWhiteSpace(_reCaptchaOptions.SECRET_KEY))
                throw new ArgumentNullException("Error con el proveedor del correos.");

            return await _reCaptchaService.ReCaptchaValidation(reCaptchaResponse, _reCaptchaOptions.SECRET_KEY);
        }

        [HttpPost("")]
        public async Task<IActionResult> SendEmail([FromForm] SendEmailRequestDTO sendEmailRequestDTO)
        {
            await _emailSenderService.SendContactMessage(sendEmailRequestDTO);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.ReCaptchas;
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
        private readonly IReCaptchasService _reCaptchasService;
        private readonly IEmailsSenderService _emailsSenderService;

        public EmailController(IEmailsSenderService emailsSenderService, IOptions<ReCaptchaOptions> options, IReCaptchasService reCaptchasService)
        {
            _reCaptchaOptions = options.Value;
            _reCaptchasService = reCaptchasService;
            _emailsSenderService = emailsSenderService;
        }

        [HttpGet("re-captcha")]
        public async Task<bool> GetReCaptcha(string reCaptchaResponse)
        {
            if (string.IsNullOrEmpty(reCaptchaResponse) && string.IsNullOrWhiteSpace(reCaptchaResponse))
                throw new ArgumentNullException("El reCaptcha ha sido nulo.");

            if (string.IsNullOrEmpty(_reCaptchaOptions.SECRET_KEY) && string.IsNullOrWhiteSpace(_reCaptchaOptions.SECRET_KEY))
                throw new ArgumentNullException("Error con el proveedor del correos.");

            return await _reCaptchasService.ReCaptchaValidation(reCaptchaResponse, _reCaptchaOptions.SECRET_KEY);
        }

        [HttpPost("")]
        public async Task<IActionResult> SendEmail([FromForm] SendEmailRequestDTO sendEmailRequestDTO)
        {
            await _emailsSenderService.SendContactMessage(sendEmailRequestDTO);
            return NoContent();
        }
    }
}

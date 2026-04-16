using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using AsadaLisboaBackend.Models.DTOs.ReCaptcha;
using AsadaLisboaBackend.ServiceContracts.Emails;
using AsadaLisboaBackend.ServiceContracts.ReCaptchas;
using AsadaLisboaBackend.Models.DTOs.InformationMessage;

namespace AsadaLisboaBackend.Areas.Auth.Controllers
{
    /// <summary>
    /// Controller for manage emails.
    /// </summary>
    [ApiController]
    [EnableRateLimiting("contact-limiter")]
    [Area("Auth")]
    [ApiVersion("1.0")]
    [Route("api/[area]/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IReCaptchasService _reCaptchasService;
        private readonly IEmailsSenderService _emailsSenderService;

        /// <summary>
        /// Constructor for EmailController.
        /// </summary>
        /// <param name="reCaptchasService"></param>
        /// <param name="emailsSenderService"></param>
        public EmailController(IEmailsSenderService emailsSenderService, IReCaptchasService reCaptchasService)
        {
            _reCaptchasService = reCaptchasService;
            _emailsSenderService = emailsSenderService;
        }

        /// <summary>
        /// Get ReCAPTCHA confirmation.
        /// </summary>
        /// <param name="reCaptchaRequestDTO">ReCAPTCHA public key.</param>
        /// <returns>ReCAPTCHA confirmation.</returns>
        [HttpPost("re-captcha")]
        public async Task<bool> GetReCaptcha([FromBody] ReCaptchaRequestDTO reCaptchaRequestDTO)
        {
            return await _reCaptchasService.ReCaptchaValidation(reCaptchaRequestDTO.ReCaptchaRequest);
        }

        /// <summary>
        /// Send a communication email (client to ASADA).
        /// </summary>
        /// <param name="sendEmailRequestDTO">An object containing the details of the email to be send.</param>
        /// <returns>No content.</returns>
        [HttpPost("")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequestDTO sendEmailRequestDTO)
        {
            await _emailsSenderService.SendContactMessage(sendEmailRequestDTO);
            return NoContent();
        }
    }
}

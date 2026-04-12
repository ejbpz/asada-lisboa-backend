using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.DTOs.ReCaptcha;
using AsadaLisboaBackend.ServiceContracts.ReCaptchas;

namespace AsadaLisboaBackend.Services.ReCaptchas
{
    public class ReCaptchasService : IReCaptchasService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReCaptchasService> _logger;
        private readonly ReCaptchaOptions _reCaptchaOptions;

        public ReCaptchasService(HttpClient httpClient, ILogger<ReCaptchasService> logger, IOptions<ReCaptchaOptions> options)
        {
            _logger = logger;
            _httpClient = httpClient;
            _reCaptchaOptions = options.Value; ;
        }

        public async Task<bool> ReCaptchaValidation(string reCaptchaRequest)
        {
            if (string.IsNullOrEmpty(reCaptchaRequest) && string.IsNullOrWhiteSpace(reCaptchaRequest))
                throw new ArgumentNullException("El reCaptcha ha sido nulo.");

            if (string.IsNullOrEmpty(_reCaptchaOptions.SECRET_KEY) && string.IsNullOrWhiteSpace(_reCaptchaOptions.SECRET_KEY))
                throw new ArgumentNullException("Error con el proveedor del correos.");

            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "secret", _reCaptchaOptions.SECRET_KEY },
                { "response", reCaptchaRequest },
            });

            var response = await _httpClient.PostAsync(Constants.DOMAIN_RECAPTCHA, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error al validar ReCAPTCHA, codigo HTTP dado: {httpStatusCode}", response.StatusCode);
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<ReCaptchaResponse>();

            if (result is null)
            {
                _logger.LogError("Error al validar ReCAPTCHA, respuesta no pudo ser deserializada");
                return false;
            }

            return result.Success;
        }
    }
}

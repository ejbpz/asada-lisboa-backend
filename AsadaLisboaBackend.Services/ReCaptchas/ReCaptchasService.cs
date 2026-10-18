using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.Models.DTOs.ReCaptcha;
using AsadaLisboaBackend.ServiceContracts.ReCaptchas;

namespace AsadaLisboaBackend.Services.ReCaptchas;

public class ReCaptchasService(
    HttpClient httpClient,
    ILogger<ReCaptchasService> logger,
    IOptions<ReCaptchaOptions> options)
    : IReCaptchasService
{
    private readonly ReCaptchaOptions _reCaptchaOptions = options.Value;

    public async Task<bool> ReCaptchaValidation(string reCaptchaRequest)
    {
        if (string.IsNullOrEmpty(reCaptchaRequest) || string.IsNullOrWhiteSpace(reCaptchaRequest))
            throw new ArgumentException("El reCaptcha ha sido nulo.");

        if (string.IsNullOrEmpty(_reCaptchaOptions.SecretKey) || string.IsNullOrWhiteSpace(_reCaptchaOptions.SecretKey))
            throw new ArgumentException("Error con el proveedor del correos.");

        var content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            { "secret", _reCaptchaOptions.SecretKey },
            { "response", reCaptchaRequest },
        });

        var response = await httpClient.PostAsync(Constants.DOMAIN_RECAPTCHA, content);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Error al validar ReCAPTCHA, codigo HTTP dado: {httpStatusCode}", response.StatusCode);
            return false;
        }

        try
        {
            var result = await response.Content
                .ReadFromJsonAsync<ReCaptchaResponseDto>();

            if (result is null)
            {
                logger.LogError("Error al validar ReCAPTCHA, respuesta no pudo ser deserializada");
                return false;
            }

            return result.Success;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex,
                "Error al deserializar la respuesta de ReCAPTCHA");

            return false;
        }
    }
}
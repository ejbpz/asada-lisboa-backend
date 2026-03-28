using System.Net.Http.Json;
using AsadaLisboaBackend.Utils;
using AsadaLisboaBackend.Models.DTOs.ReCaptcha;
using AsadaLisboaBackend.ServiceContracts.ReCaptcha;

namespace AsadaLisboaBackend.Services.ReCaptcha
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly HttpClient _httpClient;

        public ReCaptchaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReCaptchaValidation(string reCaptchaResponse, string reCaptchaSecretKey)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "secret", reCaptchaSecretKey },
                { "response", reCaptchaResponse },
            });

            var response = await _httpClient.PostAsync(Constants.DOMAIN_RECAPTCHA, content);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<ReCaptchaResponse>();

            if (result is null)
                return false;

            return result.Success;
        }
    }
}

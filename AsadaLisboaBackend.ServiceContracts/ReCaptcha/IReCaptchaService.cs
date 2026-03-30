namespace AsadaLisboaBackend.ServiceContracts.ReCaptcha
{
    public interface IReCaptchaService
    {
        public Task<bool> ReCaptchaValidation(string reCaptchaResponse, string reCaptchaSecretKey);
    }
}

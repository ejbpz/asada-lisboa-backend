namespace AsadaLisboaBackend.ServiceContracts.ReCaptchas
{
    public interface IReCaptchasService
    {
        public Task<bool> ReCaptchaValidation(string reCaptchaResponse);
    }
}

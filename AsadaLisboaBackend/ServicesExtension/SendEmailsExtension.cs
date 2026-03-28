using Resend;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class SendEmailsExtension
    {
        public static IServiceCollection SendEmailsRegistration(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                o.ApiToken = Environment.GetEnvironmentVariable(Constants.RESEND_API_TOKEN.Trim())!;
            });
            services.AddTransient<IResend, ResendClient>();

            return services;
        }
    }
}

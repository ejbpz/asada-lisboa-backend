using Resend;
using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Send email extension method.
    /// </summary>
    public static class SendEmailsExtension
    {
        /// <summary>
        /// Resend configuration registration into services and resend services added into DI container.
        /// </summary>        
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection SendEmailsRegistration(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(o =>
            {
                // Get Resend API token from environments.
                o.ApiToken = Environment.GetEnvironmentVariable(Constants.RESEND_API_TOKEN.Trim())!;
            });
            services.AddTransient<IResend, ResendClient>();

            return services;
        }
    }
}

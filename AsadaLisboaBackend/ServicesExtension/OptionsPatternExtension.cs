using AsadaLisboaBackend.Utils.OptionsPattern;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Options pattern extension method.
    /// </summary>
    public static class OptionsPatternExtension
    {
        /// <summary>
        /// OptionsPattern registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Access to the system configurations.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection OptionsPatternRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            services.Configure<ReCaptchaOptions>(configuration.GetSection("ReCaptchaOptions"));
            services.Configure<RefreshJwtOptions>(configuration.GetSection("RefreshJwtOptions"));
            services.Configure<DefaultUserOptions>(configuration.GetSection("DefaultUserOptions"));
            services.Configure<ContactEmailOptions>(configuration.GetSection("ContactEmailOptions"));

            return services;
        }
    }
}

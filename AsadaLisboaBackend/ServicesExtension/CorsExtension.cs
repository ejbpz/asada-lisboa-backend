using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to CORS.
    /// </summary>
    public static class CorsExtension
    {
        /// <summary>
        /// CORS registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection CorsRegistration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder => {
                    policyBuilder.WithOrigins(Constants.CLIENT_HOST)
                        .WithMethods(Constants.ALLOWED_HTTP_METHODS)
                        .WithHeaders(Constants.ALLOWED_HTTP_HEADERS);
                });
            });

            return services;
        }
    }
}

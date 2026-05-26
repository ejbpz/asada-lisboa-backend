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
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            return services;
        }
    }
}

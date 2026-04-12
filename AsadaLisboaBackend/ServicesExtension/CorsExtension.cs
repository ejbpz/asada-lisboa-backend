using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class CorsExtension
    {
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

using Microsoft.AspNetCore.HttpOverrides;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to Forwarded Headers.
    /// </summary>
    public static class ForwardedHeadersExtension
    {
        /// <summary>
        /// Registers the Forwarded Headers middleware.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the middleware to.</param>
        /// <returns>The IServiceCollection with the middleware added.</returns>
        public static IServiceCollection ForwardedHeadersRegistration(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedHost |
                    ForwardedHeaders.XForwardedProto;

                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            return services;
        }
    }
}

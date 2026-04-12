using AsadaLisboaBackend.Utils;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to Policies Authorization.
    /// </summary>
    public static class AuthorizationsExtension
    {
        /// <summary>
        /// Authorization policies registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection AuthorizationsRegistration(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.ROLE_LECTOR, p => p.RequireRole(Constants.ROLE_LECTOR, Constants.ROLE_EDITOR, Constants.ROLE_ADMINISTRADOR));
                options.AddPolicy(Constants.ROLE_EDITOR, p => p.RequireRole(Constants.ROLE_EDITOR, Constants.ROLE_ADMINISTRADOR));
                options.AddPolicy(Constants.ROLE_ADMINISTRADOR, p => p.RequireRole(Constants.ROLE_ADMINISTRADOR));
            });
            return services;
        }
    }
}

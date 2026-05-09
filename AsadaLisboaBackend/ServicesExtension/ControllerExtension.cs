using AsadaLisboaBackend.Conventions;
using AsadaLisboaBackend.Models.Filters.ActionFilter;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to controller.
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// Controller registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection ControllerRegistration(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<TrimStringFilter>();
                options.Conventions.Add(new AuthorizationConvention());
            });

            return services;
        }
    }
}

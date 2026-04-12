using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to database.
    /// </summary>
    public static class DatabasesExtension
    {
        /// <summary>
        /// Database registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Access to the system configurations.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection DatabasesRegistration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AsadaLisboaDB"));
            });

            return services;
        }
    }
}

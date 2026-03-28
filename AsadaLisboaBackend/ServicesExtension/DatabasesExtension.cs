using Microsoft.EntityFrameworkCore;
using AsadaLisboaBackend.Models.DatabaseContext;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class DatabasesExtension
    {
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

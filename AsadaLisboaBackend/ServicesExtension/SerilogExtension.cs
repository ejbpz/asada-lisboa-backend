using Serilog;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to Serilog.
    /// </summary>
    public static class SerilogExtension
    {
        /// <summary>
        /// Serilog registration into services, reading params from configurations to log into console and file.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="host">Read configurations to initialize data.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection SerilogRegistration(this IServiceCollection services, ConfigureHostBuilder host)
        {
            host.UseSerilog((context, services, logger) =>
            {
                logger.ReadFrom.Configuration(context.Configuration);
                logger.ReadFrom.Services(services);
            });

            return services;
        }
    }
}

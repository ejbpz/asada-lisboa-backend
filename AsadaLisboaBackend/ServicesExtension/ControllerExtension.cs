using Microsoft.AspNetCore.Mvc;
using AsadaLisboaBackend.Conventions;

namespace AsadaLisboaBackend.ServicesExtension
{
    public static class ControllerExtension
    {
        public static IServiceCollection ControllerRegistration(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new AuthorizationConvention());
            });

            return services;
        }
    }
}

using Asp.Versioning;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to API Versioning.
    /// </summary>
    public static class VersioningExtension
    {
        /// <summary>
        /// API Versioning registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection VersioningRegistration(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = new HeaderApiVersionReader("x-version");
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}

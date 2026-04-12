using System.Reflection; 
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using AsadaLisboaBackend.Filters.Authorize;

namespace AsadaLisboaBackend.ServicesExtension
{
    /// <summary>
    /// Extension method to Swagger options.
    /// </summary>
    public class SwaggerOptionsExtension : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// SwaggerOptionsExtension Controller.
        /// </summary>
        /// <param name="provider">Provide information about API Versioning.</param>
        public SwaggerOptionsExtension(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configure the API Versioning into Swagger.
        /// </summary>
        /// <param name="options">Swagger options generation.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = ".NET Core 8 - API v1",
                Title = "ASADA de Urbanización Lisboa - Web API",
                Description = "Versioning Web API to ASADA de Urbanización Lisboa",
            };
            if (description.IsDeprecated)
            {
                info.Description += "This API version has been deprecated.";
            }
            return info;
        }
    }

    /// <summary>
    /// Extension method to Swagger.
    /// </summary>
    public static class SwaggerRegistrationExtension
    {
        /// <summary>
        /// Swagger registration into services.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <returns>List of registered services.</returns>
        public static IServiceCollection SwaggerRegistration(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptionsExtension>();

            services.AddSwaggerGen(c =>
            {
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme.",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }
    }
}

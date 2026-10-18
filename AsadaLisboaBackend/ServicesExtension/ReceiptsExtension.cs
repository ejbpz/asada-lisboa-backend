using System.Net;
using AsadaLisboaBackend.Services.Receipts;
using AsadaLisboaBackend.Utils.OptionsPattern;
using AsadaLisboaBackend.ServiceContracts.Receipts;

namespace AsadaLisboaBackend.ServicesExtension;

/// <summary>
/// Service extension for receipts services, such as getting receipt content and details.
/// </summary>
public static class ReceiptsExtension
{
    /// <summary>
    /// Registration of receipts services into services collection.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <param name="configuration">Access to the system configurations.</param>
    /// <returns>List of registered services.</returns>
    public static IServiceCollection ReceiptsServicesRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ReceiptOptions>(configuration.GetSection("ReceiptOptions"));
            
        services.AddScoped<IReceiptsGetterService, ReceiptsGetterService>();
        services.AddSingleton<CookieContainer>();

        services.AddHttpClient<IReceiptsGetterService, ReceiptsGetterService>()
            .ConfigurePrimaryHttpMessageHandler((sp) => new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = sp.GetRequiredService<CookieContainer>(),
                AutomaticDecompression = DecompressionMethods.All
            });

        return services;
    }
}
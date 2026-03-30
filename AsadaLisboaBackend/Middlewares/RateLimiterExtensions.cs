using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace AsadaLisboaBackend.Middlewares
{
    public static class RateLimiterExtensions
    {
        public static IServiceCollection AddContactRateLimiters(this IServiceCollection services)
        {
            services.AddRateLimiter(options => {
                options.AddFixedWindowLimiter("contact-limiter", limiterOptions => 
                {
                    limiterOptions.PermitLimit = 3;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });

                options.OnRejected = (async (context, cancellationToken) => 
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await
                    context.HttpContext.Response.WriteAsJsonAsync(
                        new {
                            title = "Excedido el número de llamadas por minuto.",
                            detail = "Demasiadas solicitudes realizadas.",
                            status = 429,
                            traceId = context.HttpContext.TraceIdentifier,
                        },
                        cancellationToken
                    );
                });
            });

            return services;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class SecurityTokenErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<SecurityTokenErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not SecurityTokenException) 
            return false;

        logger.LogError(exception, "Error Global - Error al validar token de acceso.");

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error validación de token",
                Status = StatusCodes.Status401Unauthorized,
            }
        });

        return true;
    }
}
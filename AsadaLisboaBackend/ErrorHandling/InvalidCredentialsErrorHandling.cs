using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class InvalidCredentialsErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<InvalidCredentialsErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidCredentialsException) 
            return false;

        logger.LogError(exception, "Error Global - Error en credenciales de acceso de un usuario.");

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error en credenciales de usuario",
                Status = StatusCodes.Status401Unauthorized,
            }
        });

        return true;
    }
}
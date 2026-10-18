using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class InvalidAccessTokenErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<InvalidAccessTokenErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidAccessTokenException) 
            return false;

        logger.LogError(exception, "Error Global - Error en autenticación de token.");

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error en autenticación de token",
                Status = StatusCodes.Status401Unauthorized,
            }
        });

        return true;
    }
}
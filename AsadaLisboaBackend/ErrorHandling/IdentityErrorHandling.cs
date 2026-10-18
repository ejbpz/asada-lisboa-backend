using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class IdentityErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<IdentityErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not IdentityErrorException identityErrorException) 
            return false;

        logger.LogError(exception, "Error Global - Error al realizar cambios de un usuario.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Detail = exception.Message,
            Status = StatusCodes.Status400BadRequest,
            Title = "Error al hacer cambios del usuario",
            Extensions =
            {
                ["errors"] = identityErrorException.Errors
            }
        };

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        return true;
    }
}
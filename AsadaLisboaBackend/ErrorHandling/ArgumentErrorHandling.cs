using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class ArgumentErrorHandling
(IProblemDetailsService problemDetailsService, 
    ILogger<ArgumentErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ArgumentException)
            return false;

        logger.LogError(exception, "Error Global - Error en el valor enviado.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error en el valor enviado",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class GlobalErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        logger.LogError(exception, "Error Global - Error no controlado por el sistema.");

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Title = "Error interno del servidor.",
                Detail = exception.Message,
            }
        });

        return true;
    }
}
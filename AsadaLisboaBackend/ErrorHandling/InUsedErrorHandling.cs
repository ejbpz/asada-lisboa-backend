using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class InUsedErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<InUsedErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InUsedException)
            return false;

        logger.LogError(exception, "Error Global - Elemento al eliminar una categoría en uso.");

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error al utilizar el recurso",
                Status = StatusCodes.Status409Conflict,
            }
        });

        return true;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class NotFoundErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<NotFoundErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException) 
            return false;

        logger.LogError(exception, "Error Global - Elemento buscado no encontrado.");

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error en búsqueda",
                Status = StatusCodes.Status404NotFound,
            }
        });

        return true;
    }
}
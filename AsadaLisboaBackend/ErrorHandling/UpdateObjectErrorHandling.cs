using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class UpdateObjectErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<UpdateObjectErrorHandling> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not UpdateObjectException)
            return false;

        logger.LogError(exception, "Error Global - Error al actualizar un valor.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error al actualizar un valor",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}
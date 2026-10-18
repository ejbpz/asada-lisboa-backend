using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AsadaLisboaBackend.Services.Exceptions;

namespace AsadaLisboaBackend.ErrorHandling;

internal sealed class CreateObjectErrorHandling(
    IProblemDetailsService problemDetailsService,
    ILogger<CreateObjectErrorHandling> logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not CreateObjectException)
            return false;

        logger.LogError(exception, "Error Global - Error no al crear un elemento en el sistema.");

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await problemDetailsService.TryWriteAsync(new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Detail = exception.Message,
                Title = "Error al crear un valor",
                Status = StatusCodes.Status400BadRequest,
            }
        });

        return true;
    }
}